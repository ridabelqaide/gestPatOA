import { Component, OnInit } from '@angular/core';
import { AutoService } from '../../Services/auto-list.service';
import { Auto } from '../../models/auto.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Document, Packer, Paragraph, Table, TableCell, TableRow, TextRun, WidthType } from 'docx';
import { saveAs } from 'file-saver';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-automobile-list',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './automobile-list.component.html',
  styleUrls: ['./automobile-list.component.css']
})
export class AutomobileListComponent implements OnInit {
  autos: Auto[] = [];
  filteredAutos: Auto[] = [];
  searchText: string = '';
  showModal: boolean = false;
  editingAuto: Auto | null = null;
  autoForm: Auto = this.resetForm();
  errors: any = {};

  constructor(private autoService: AutoService) { }

  ngOnInit(): void {
    this.loadAutos();
  }

  loadAutos() {
    this.autoService.getAutos().subscribe({
      next: (data) => {
        this.autos = data;
        this.filteredAutos = [...data];
      },
      error: (err) => alert('Erreur lors du chargement des véhicules')
    });
  }

  resetForm(): Auto {
    return {
      matricule: '',
      genre: '',
      type: '',
      marque: '',
      model: '',
      modeCarburant: '',
      acquisition: '',
      miseCirculationDate: new Date().toISOString(),
      etat: '',
      th: 0,
      tj: 0
    };
  }

  openAddModal() {
    this.autoForm = this.resetForm();
    this.editingAuto = null;
    this.showModal = true;
  }

  openEditModal(auto: Auto) {
    this.editingAuto = auto;
    this.autoForm = { ...auto };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.editingAuto = null;
  }

 

  isFormValid: boolean = false;

  validateForm(): boolean {
    this.errors = {};

    if (!this.autoForm.matricule || this.autoForm.matricule.trim() === '') {
      this.errors.matricule = 'Le matricule est obligatoire';
    }

    if (!this.autoForm.genre || this.autoForm.genre.trim() === '') {
      this.errors.genre = 'Le genre est obligatoire';
    }

    if (!this.autoForm.type || this.autoForm.type.trim() === '') {
      this.errors.type = 'Le type est obligatoire';
    }

    if (!this.autoForm.marque || this.autoForm.marque.trim() === '') {
      this.errors.marque = 'La marque est obligatoire';
    }

    if (!this.autoForm.modeCarburant || this.autoForm.modeCarburant.trim() === '') {
      this.errors.modeCarburant = 'Le mode de carburant est obligatoire';
    }

    if (!this.autoForm.acquisition || this.autoForm.acquisition.trim() === '') {
      this.errors.acquisition = 'L\'acquisition est obligatoire';
    }

    if (!this.autoForm.miseCirculationDate) { 
      this.errors.miseCirculationDate = 'La date de mise en service est obligatoire';
    }

    if (!this.autoForm.etat || this.autoForm.etat.trim() === '') {
      this.errors.etat = 'L\'état du véhicule est obligatoire';
    }

    this.isFormValid = Object.keys(this.errors).length === 0;
    return this.isFormValid;
  }



saveAuto() {
  if (!this.validateForm()) {
    Swal.fire({
      icon: 'error',
      title: 'Formulaire invalide',
      text: 'Veuillez remplir tous les champs obligatoires !'
    });
    return;
  }

  if (this.editingAuto) {
    this.autoService.updateAuto(this.autoForm).subscribe({
      next: () => {
        const index = this.autos.findIndex(a => a.id === this.autoForm.id);
        if (index !== -1) this.autos[index] = { ...this.autoForm };
        this.filteredAutos = [...this.autos];
        this.closeModal();
        Swal.fire({
          icon: 'success',
          title: 'Succès',
          text: 'Véhicule mis à jour !'
        });
      },
      error: () => Swal.fire({ icon: 'error', title: 'Erreur', text: 'Erreur lors de la mise à jour' })
    });
  } else {
    this.autoService.addAuto(this.autoForm).subscribe({
      next: (res) => {
        this.autos.push(res);
        this.filteredAutos = [...this.autos];
        this.closeModal();
        Swal.fire({
          icon: 'success',
          title: 'Succès',
          text: 'Véhicule ajouté !'
        });
      },
      error: () => Swal.fire({ icon: 'error', title: 'Erreur', text: 'Erreur lors de l\'ajout' })
    });
  }
}


  deleteAuto(id: string) {
    Swal.fire({
      title: 'Êtes-vous sûr ?',
      text: "Voulez-vous vraiment supprimer ce véhicule ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Oui, supprimer',
      cancelButtonText: 'Annuler'
    }).then((result) => {
      if (result.isConfirmed) {
        this.autoService.deleteAuto(id).subscribe({
          next: () => {
            this.autos = this.autos.filter(a => a.id !== id);
            this.filteredAutos = [...this.autos];
            Swal.fire('Supprimé !', 'Véhicule supprimé avec succès.', 'success');
          },
          error: () => Swal.fire('Erreur', 'Erreur lors de la suppression du véhicule', 'error')
        });
      }
    });
  }


  search() {
    const text = this.searchText.toLowerCase();
    this.filteredAutos = this.autos.filter(a =>
      a.matricule.toLowerCase().includes(text) ||
      a.marque.toLowerCase().includes(text)
    );
  }

  exportToWord() {
    const vehicules = this.autos;
    if (vehicules.length === 0) {
      alert('Aucun véhicule à exporter');
      return;
    }

    const tableRows: TableRow[] = [];
    const headers = ["N", "Matricule", "Genre", "Type", "Marque", "Carburant", "Acquisition", "Date Mise Circulation", "État"];

    tableRows.push(
      new TableRow({
        children: headers.map(h =>
          new TableCell({ children: [new Paragraph({ text: h })] })
        ),
      })
    );

    vehicules.forEach((v, i) => {
      tableRows.push(
        new TableRow({
          children: [
            new TableCell({ children: [new Paragraph(String(i + 1))] }),
            new TableCell({ children: [new Paragraph(v.matricule || '')] }),
            new TableCell({ children: [new Paragraph(v.genre || '')] }),
            new TableCell({ children: [new Paragraph(v.type || '')] }),
            new TableCell({ children: [new Paragraph(v.marque || '')] }),
            new TableCell({ children: [new Paragraph(v.modeCarburant || '')] }),
            new TableCell({ children: [new Paragraph(v.acquisition || '')] }),
            new TableCell({ children: [new Paragraph(new Date(v.miseCirculationDate).toLocaleDateString())] }),
            new TableCell({ children: [new Paragraph(v.etat || '')] }),
          ],
        })
      );
    });

    const doc = new Document({
      sections: [{
        children: [
          new Paragraph({ children: [new TextRun({ text: "Liste des Véhicules", bold: true, size: 36 })], spacing: { after: 400 } }),
          new Table({ width: { size: 100, type: WidthType.PERCENTAGE }, rows: tableRows }),
        ],
      }],
    });

    Packer.toBlob(doc).then(blob => {
      saveAs(blob, "Liste_Vehicules.docx");
      Swal.fire('Export Word', 'Export Word effectué avec succès !', 'success');
    });
  }

}
