import { Component, OnInit } from '@angular/core';
import { OfficialService } from '../../Services/official.service';
import { Official } from '../../models/Official.model';
import { AuthService } from '../../Services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatPaginatorModule, PageEvent, MatPaginatorIntl } from '@angular/material/paginator';

@Component({
  selector: 'app-official',
  standalone: true,
  imports: [FormsModule, CommonModule, MatPaginatorModule ,],
  templateUrl: './official.component.html',
  styleUrls: ['./official.component.css']
})
export class OfficialComponent implements OnInit {

  officials: Official[] = [];
  page = 1;
  pageSize = 5;
  totalItems = 0;
  showModal = false;
  editOfficial: any = {};
  isAdmin = false;
  filters: any = {
    genre: '',
    fonction: '',
    service: '',
  };

  constructor(
    private officialService: OfficialService,
    private authService: AuthService,
    private toastr: ToastrService,
    private paginatorIntl: MatPaginatorIntl
  ) {
    this.paginatorIntl.itemsPerPageLabel = 'Éléments par page';
    this.paginatorIntl.nextPageLabel = 'Suivant';
    this.paginatorIntl.previousPageLabel = 'Précédent';
    this.paginatorIntl.firstPageLabel = 'Premier';
    this.paginatorIntl.lastPageLabel = 'Dernier';
  }

  ngOnInit(): void {
    this.checkAdmin();
    this.loadOfficials();
  }

  checkAdmin() {
    const role = this.authService.getRole();
    this.isAdmin = role === 'admin';
  }

  onFilterChange(field: string, value: string) {
    this.filters[field] = value;
    this.loadOfficials();
  }

  loadOfficials() {
    const { genre, fonction, service } = this.filters;
    this.officialService.getPaged(
      genre || '', fonction || '', service || '',
      this.page, this.pageSize
    ).subscribe({
      next: res => {
        this.officials = res.data;
        this.totalItems = res.totalItems;
      },
      error: err => this.toastr.error('Erreur lors du chargement des officiels')
    });
  }

  openEditModal(official: Official) {
    this.editOfficial = { ...official };
    this.showModal = true;
  }
  openAddModal() {
    this.editOfficial = {
      cin: '',
      name: '',
      genre: '',
      fonction: '',
      service: ''
    };
    this.showModal = true;
  }

  closeEditModal() {
    this.showModal = false;
  }
  resetFilters(): void {
    this.filters = { genre: '', fonction: '', service: '' };
    this.page = 1;
    this.loadOfficials();
  }
  saveEdit() {
    if (this.editOfficial && this.editOfficial.id) {
      this.officialService.update(this.editOfficial.id, this.editOfficial)
        .subscribe({
          next: () => {
            this.showModal = false;
            this.loadOfficials();
            this.toastr.success("Officiel mis à jour !");
          },
          error: () => this.toastr.error("Erreur lors de la mise à jour"),
        });
      return;
    }

    this.officialService.create(this.editOfficial)
      .subscribe({
        next: () => {
          this.showModal = false;
          this.loadOfficials();
          this.toastr.success("Officiel ajouté avec succès !");
        },
        error: () => this.toastr.error("Erreur lors de l'ajout"),
      });
  }

  deleteOfficial(id?: string) {

    this.officialService.delete(id!).subscribe({
      next: () => {
        this.toastr.success('Officiel supprimé avec succès');
        this.loadOfficials();
      },
      error: err => {
        console.error(err);
        this.toastr.error('Erreur lors de la suppression');
      }
    });
  }

  onPageChange(event: PageEvent) {
    this.page = event.pageIndex + 1; 
    this.pageSize = event.pageSize;
    this.loadOfficials();
  }
}
