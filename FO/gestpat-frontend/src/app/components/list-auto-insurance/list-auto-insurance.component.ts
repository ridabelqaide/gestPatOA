import { Component, OnInit } from '@angular/core';
import { AutoService } from '../../Services/auto-list.service';
import { InsuranceService } from '../../Services/insurance.service';
import { Auto } from '../../models/auto.model';
import { Insurance } from '../../models/insurance.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import * as FileSaver from 'file-saver';
import * as docx from 'docx';
import { TableRow } from 'docx';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-list-auto-insurance',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
  templateUrl: './list-auto-insurance.component.html',
  styleUrls: ['./list-auto-insurance.component.css']
})
export class ListAutoInsuranceComponent implements OnInit {

  autos: Auto[] = [];
  filteredAutos: Auto[] = [];
  searchText: string = '';
  showModal: boolean = false;
  editingInsurance: Insurance | null = null;
  insuranceForm: Insurance = this.resetForm();
  loading = true;
  errors: any = {};
  vehicules: any[] = [];
  totalItems = 0;
  page = 1;
  pageSize = 5;

  filters: any = {
    matricule: '',
    type: '',
    company: '',
    date: ''
  };
  Types: string[] = [
    'Responsabilité Civile', 'Tous Risques', 'Dommages Collision', 'Vol et Incendie',
    'Bris de Glace', 'Catastrophes Naturelles', 'Assistance Routière',
    'Conducteur Supplémentaire', 'Transport Marchandises', 'Taxi / Professionnelle'
  ];

  Companies: string[] = [
    'Wafa Assurance', 'RMA (Royale Marocaine d’Assurance)', 'AXA Assurance Maroc', 'AtlantaSanad Assurance',
    'MCMA (Mutuelle Centrale Marocaine d’Assurance)', 'Allianz Maroc', 'Saham Assurance', 'MAMDA',
    'CNIA Saada', 'La Marocaine Vie', 'Zurich Maroc', 'Afriquia Assurance'
  ];


  constructor(
    private autoService: AutoService,
    private insuranceService: InsuranceService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.loadAutosWithLastInsurance();
    this.autoService.getAll().subscribe(data => {
      this.vehicules = data;
    });
  }
  refreshList(): void {
    this.loadAutosWithLastInsurance();
  }
  resetFilters(): void {
    this.filters = {};
    this.searchText = '';
    this.loadAutosWithLastInsurance();
  }
  loadAutosWithLastInsurance() {
    const allFilters = { ...this.filters, page: this.page, pageSize: this.pageSize };
    this.autoService.getLastInsurance(allFilters).subscribe({
      next: (res: any) => {
        const dataArray = Array.isArray(res.data) ? res.data : [];
        this.totalItems = res.totalItems; // ← ici le total réel

        this.autos = dataArray.map((item: any) => ({
          id: item.enginId,
          matricule: item.matricule,
          insurance: {
            id: item.id,
            company: item.company,
            type: item.type,
            enginId: item.enginId,
            amount: item.amount,
            startDate: new Date(item.startDate),
            endDate: new Date(item.endDate),
            matricule: item.matricule
          }
        }));

        this.filteredAutos = [...this.autos];
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  onFilterChange(field: string, value: string) {
    this.filters[field] = value;
    this.loadAutosWithLastInsurance();
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadAutosWithLastInsurance();
  }
  search(): void {
    this.page = 1;
    this.filters['matricule'] = this.searchText.trim();
    this.loadAutosWithLastInsurance();
  }

  resetForm(): Insurance {
    return {
      company: '',
      matricule: '',
      type: '',
      enginId: '',
      amount: 0,
      startDate: new Date(),
      endDate: new Date()
    };
  }

  openAddModal(auto?: Auto) {
    this.insuranceForm = this.resetForm();
    if (auto) {
      this.insuranceForm.matricule = auto.matricule;
    }
    this.editingInsurance = null;
    this.showModal = true;
  }

  openEditModal(insurance: Insurance) {
    this.editingInsurance = insurance;
    this.insuranceForm = {
      company: insurance.company,
      type: insurance.type,
      amount: insurance.amount,
      enginId: insurance.enginId,
      startDate: new Date(insurance.startDate),
      endDate: new Date(insurance.endDate),
      matricule: insurance.matricule || ''
    };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingInsurance = null;
  }

  validateForm(): boolean {
    this.errors = {};
    if (!this.insuranceForm.company) this.errors.company = 'Compagnie requise';
    if (!this.insuranceForm.type) this.errors.type = 'Type requis';
    if (!this.insuranceForm.amount || this.insuranceForm.amount <= 0)
      this.errors.amount = 'Montant invalide';
    if (!this.insuranceForm.startDate)
      this.errors.startDate = 'Date début requise';
    if (!this.insuranceForm.endDate)
      this.errors.endDate = 'Date fin requise';
    return Object.keys(this.errors).length === 0;
  }

  saveInsurance() {
    if (!this.validateForm()) {
      this.toastr.error('Veuillez remplir tous les champs correctement');
      return;
    }

    const auto = this.vehicules.find(v => v.matricule === this.insuranceForm.matricule);

    const insuranceData: Insurance = {
      id: this.editingInsurance?.id,
      company: this.insuranceForm.company,
      type: this.insuranceForm.type,
      amount: this.insuranceForm.amount,
      startDate: this.insuranceForm.startDate,
      endDate: this.insuranceForm.endDate,
      enginId: auto ? auto.id : '',
      matricule: this.insuranceForm.matricule!
    };

    const isEdit = !!this.editingInsurance; 
    const obs = isEdit
      ? this.insuranceService.update(this.editingInsurance!.id!, insuranceData)
      : this.insuranceService.create(insuranceData);

    obs.subscribe({
      next: () => {
        this.loadAutosWithLastInsurance();
        this.closeModal();
        this.toastr.success(isEdit ? 'Assurance modifiée avec succès' : 'Assurance ajoutée avec succès');
      },
      error: (err) => {
        console.error(err);
        this.toastr.error('Problème lors de l\'enregistrement');
      }
    });
  }


  deleteInsurance(id?: string) {
    if (!id) return;

    this.insuranceService.delete(id).subscribe({
      next: () => {
        this.loadAutosWithLastInsurance();
        this.toastr.success('Assurance supprimée avec succès');
      },
      error: () => this.toastr.error('Erreur lors de la suppression')
    });
  }
  exportToWord() {
    this.autoService.getLastInsuranceAll().subscribe({
      next: (data: any[]) => {
        this.generateWordDoc(data);
      },
      error: err => this.toastr.error('Erreur lors du chargement des véhicules', 'Erreur')
    });
  }

  generateWordDoc(autos: any[]) {
    const { Document, Packer, Paragraph, Table, TableRow, TableCell } = docx;

    const tableRows: TableRow[] = [
      new TableRow({
        children: ['N°', 'Véhicule', 'Compagnie', 'Type', 'Montant', 'Début', 'Fin']
          .map(header => new TableCell({ children: [new Paragraph({ text: header })] }))
      })
    ];

    autos.forEach((a, i) => {
      tableRows.push(new TableRow({
        children: [
          new TableCell({ children: [new Paragraph(String(i + 1))] }),
          new TableCell({ children: [new Paragraph(a.matricule)] }),
          new TableCell({ children: [new Paragraph(a.company || '-')] }),
          new TableCell({ children: [new Paragraph(a.type || '-')] }),
          new TableCell({ children: [new Paragraph(String(a.amount || '-'))] }),
          new TableCell({ children: [new Paragraph(a.startDate ? new Date(a.startDate).toLocaleDateString() : '-')] }),
          new TableCell({ children: [new Paragraph(a.endDate ? new Date(a.endDate).toLocaleDateString() : '-')] }),
        ]
      }));
    });

    const doc = new Document({ sections: [{ children: [new Table({ rows: tableRows })] }] });

    Packer.toBlob(doc).then(blob => {
      FileSaver.saveAs(blob, 'liste_assurances_complet.docx');
      this.toastr.success('Export Word effectué avec succès !');
    });
  }
}
