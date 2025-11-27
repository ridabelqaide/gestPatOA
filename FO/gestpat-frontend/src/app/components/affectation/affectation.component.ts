import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { AffectationService } from '../../Services/affectation.service';
import { Affectation } from '../../models/Affectation';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatPaginatorModule, MatPaginatorIntl } from '@angular/material/paginator';
import { OfficialService } from '../../Services/official.service';
import { AutoService } from '../../Services/auto-list.service';


@Component({
  selector: 'app-affectation',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
  templateUrl: './affectation.component.html',
  styleUrls: ['./affectation.component.css']
})
export class AffectationComponent implements OnInit {

  affectations: Affectation[] = [];
  loading = true;
  totalItems = 0;
  page = 1;
  pageSize = 5;
  officials: any[] = [];
  engins: any[] = [];

  filters: any = {
    officialId: '',
    enginId: '',
    startDate: '',
  };

  form: Affectation = this.resetForm();
  isEditMode = false;
  showModal = false;
  constructor(
    private affectationService: AffectationService,
    private toastr: ToastrService,
    private officialService: OfficialService,
    private autoService: AutoService,
   private paginatorIntl: MatPaginatorIntl
  ) {
    this.paginatorIntl.itemsPerPageLabel = 'Éléments par page';
    this.paginatorIntl.nextPageLabel = 'Suivant';
    this.paginatorIntl.previousPageLabel = 'Précédent';
    this.paginatorIntl.firstPageLabel = 'Premier';
    this.paginatorIntl.lastPageLabel = 'Dernier';
  }


  ngOnInit(): void {
    this.loadAffectations();
    this.officialService.getAll().subscribe({
      next: res => this.officials = res,
      error: err => console.error(err)
    });

    this.autoService.getAll().subscribe({
      next: res => this.engins = res,
      error: err => console.error(err)
    });
  }

  loadAffectations() {
    this.loading = true;

    const officialId = this.filters.officialId || undefined;
    const enginId = this.filters.enginId || undefined;
    const startDate = this.filters.startDate
      ? new Date(this.filters.startDate).toISOString().split('T')[0]
      : undefined;

    this.affectationService.getPaged(
      this.page,
      this.pageSize,
      officialId,
      enginId,
      startDate
    ).subscribe({
      next: (res: any) => {
        this.affectations = res.data; 
        this.totalItems = res.totalItems;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.toastr.error('Erreur lors du chargement des affectations');
        this.loading = false;
      }
    });
  }
  resetFilters(): void {
    this.filters = {
      officialId: '',
      enginId: '',
      startDate: '',
    };
    this.page = 1; 
    this.loadAffectations();
  }

  onFilterChange(field: string, value: string) {
    this.filters[field] = value;
    this.page = 1;
    this.loadAffectations();
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadAffectations();
  }

  resetForm(): Affectation {
    return {
      id: '',
      officialId: '',
      enginId: '',
      startDate: new Date()
    };
  }
  openModal() {
    this.form = this.resetForm();
    this.isEditMode = false;
    this.showModal = true;
  }
  closeModal() {
    this.showModal = false;
    this.form = this.resetForm();
    this.isEditMode = false;
  }

  editAffectation(affectation: Affectation) {
    this.form = { ...affectation };
    this.isEditMode = true;
    this.showModal = true; 
  }

  cancelEdit() {
    this.form = this.resetForm();
    this.isEditMode = false;
  }

  saveAffectation() {
    if (!this.form.officialId || !this.form.enginId || !this.form.startDate || !this.form.object) {
      this.toastr.error('Tous les champs obligatoires doivent être remplis');
      return;
    }

    const affectationToSend: any = {
      officialId: this.form.officialId,
      enginId: this.form.enginId,
      startDate: new Date(this.form.startDate).toISOString().split('T')[0],
      endDate: this.form.endDate ? new Date(this.form.endDate).toISOString().split('T')[0] : new Date(this.form.startDate).toISOString().split('T')[0],
      currentKm: Number(this.form.currentKm || 0),
      endKm: Number(this.form.endKm || 0),
      object: this.form.object,
      details: this.form.details || '',
    };

    const obs = this.isEditMode && this.form.id
      ? this.affectationService.update(this.form.id, affectationToSend)
      : this.affectationService.create(affectationToSend);

    obs.subscribe({
      next: () => {
        this.toastr.success(this.isEditMode ? 'Affectation modifiée' : 'Affectation ajoutée');
        this.loadAffectations();
        this.cancelEdit();
        this.closeModal();
      },
      error: err => {
        console.error(err);
        this.toastr.error('Erreur lors de l\'enregistrement');
      }
    });
  }



  deleteAffectation(id?: string) {
    if (!id) return;

    this.affectationService.delete(id).subscribe({
      next: () => {
        this.toastr.success('Affectation supprimée avec succès');
        this.loadAffectations();
      },
      error: () => this.toastr.error('Erreur lors de la suppression')
    });
  }
}
