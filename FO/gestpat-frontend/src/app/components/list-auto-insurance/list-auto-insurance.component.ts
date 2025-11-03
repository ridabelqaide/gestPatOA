import { Component, OnInit } from '@angular/core';
import { AutoService } from '../../Services/auto-list.service';
import { InsuranceService } from '../../Services/insurance.service';
import { Auto } from '../../models/auto.model';
import { Insurance } from '../../models/insurance.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import * as FileSaver from 'file-saver';
import * as docx from 'docx';
import { TableRow } from 'docx';

@Component({
  selector: 'app-list-auto-insurance',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  constructor(
    private autoService: AutoService,
    private insuranceService: InsuranceService
  ) { }

  ngOnInit(): void {
    this.loadAutosWithLastInsurance();
  }

  loadAutosWithLastInsurance() {
    this.autoService.getLastInsurance().subscribe({
      next: (data: any[]) => {
        this.autos = data.map(item => {
          const insurance: Insurance = {
            id: item.id,
            company: item.company,
            type: item.type,
            enginId: item.enginId,
            amount: item.amount,
            startDate: new Date(item.startDate),
            endDate: new Date(item.endDate),
            matricule: item.matricule
          };
          return {
            id: item.enginId,
            matricule: item.matricule,
            insurance
          } as Auto;
        });

        this.filteredAutos = [...this.autos];
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.loading = false;
      }
    });
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
      Swal.fire({ icon: 'error', title: 'Erreur', text: 'Veuillez remplir tous les champs' });
      return;
    }


    const insuranceData: Insurance = {
      id: this.editingInsurance?.id,
      company: this.insuranceForm.company,
      type: this.insuranceForm.type,
      amount: this.insuranceForm.amount,
      startDate: this.insuranceForm.startDate,
      endDate: this.insuranceForm.endDate,
      enginId: this.insuranceForm.enginId,
      matricule: this.insuranceForm.matricule!
    };

    const obs = this.editingInsurance
      ? this.insuranceService.update(this.editingInsurance.id!, insuranceData)
      : this.insuranceService.create(insuranceData);

    obs.subscribe({
      next: () => {
        this.loadAutosWithLastInsurance();
        this.closeModal();
        Swal.fire({
          icon: 'success',
          title: this.editingInsurance ? 'Assurance mise à jour' : 'Assurance ajoutée'
        });
      },
      error: () => Swal.fire({ icon: 'error', title: 'Erreur', text: 'Problème lors de l\'enregistrement' })
    });
  }


  deleteInsurance(id?: string) {
    if (!id) return;
    Swal.fire({
      title: 'Supprimer cette assurance ?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Oui',
      cancelButtonText: 'Annuler'
    }).then(result => {
      if (result.isConfirmed) {
        this.insuranceService.delete(id).subscribe({
          next: () => {
            this.loadAutosWithLastInsurance();
            Swal.fire({
              icon: 'success',
              title: 'Supprimée',
              text: 'Assurance supprimée'
            });
          },
          error: () =>
            Swal.fire({
              icon: 'error',
              title: 'Erreur',
              text: 'Problème lors de la suppression'
            })
        });
      }
    });
  }

  search() {
    const query = this.searchText.trim();

    this.autoService.getLastInsurance(query).subscribe({
      next: (data: any[]) => {
        this.autos = data.map(item => {
          const insurance: Insurance = {
            id: item.id,
            company: item.company,
            type: item.type,
            amount: item.amount,
            startDate: new Date(item.startDate),
            endDate: new Date(item.endDate),
            enginId: item.enginId,
            matricule: item.matricule
          };
          return {
            id: item.enginId,
            matricule: item.matricule,
            insurance
          } as Auto;
        });

        this.filteredAutos = [...this.autos];
      },
      error: err => {
        console.error(err);
      }
    });
  }


  exportToWord() {
    const { Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell } = docx;

    const tableRows: TableRow[] = [
      new TableRow({
        children: [
          'N°', 'Véhicule', 'Compagnie', 'Type', 'Montant', 'Début', 'Fin'
        ].map(header =>
          new TableCell({ children: [new Paragraph({ text: header })] })
        )
      })
    ];

    this.autos.forEach((a, i) => {
      tableRows.push(new TableRow({
        children: [
          new TableCell({ children: [new Paragraph(String(i + 1))] }),
          new TableCell({ children: [new Paragraph(a.matricule)] }),
          new TableCell({ children: [new Paragraph(a.insurance?.company || '-')] }),
          new TableCell({ children: [new Paragraph(a.insurance?.type || '-')] }),
          new TableCell({ children: [new Paragraph(String(a.insurance?.amount || '-'))] }),
          new TableCell({ children: [new Paragraph(a.insurance?.startDate?.toLocaleDateString() || '-')] }),
          new TableCell({ children: [new Paragraph(a.insurance?.endDate?.toLocaleDateString() || '-')] }),
        ]
      }));
    });

    const doc = new Document({
      sections: [{ children: [new Table({ rows: tableRows })] }]
    });

    Packer.toBlob(doc).then(blob => {
      FileSaver.saveAs(blob, 'liste_assurances.docx');
    });
  }
}
