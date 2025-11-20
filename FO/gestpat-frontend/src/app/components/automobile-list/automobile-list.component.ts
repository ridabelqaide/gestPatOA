import { Component, OnInit } from '@angular/core';
import { AutoService } from '../../Services/auto-list.service';
import { Auto } from '../../models/auto.model';
import { PagedResult } from '../../models/paged-result.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Document, Packer, Paragraph, Table, TableCell, TableRow, TextRun, WidthType } from 'docx';
import { saveAs } from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { EnginType } from '../../models/engin-type.model';
import { EnginTypeService } from '../../Services/engin-type.service';

@Component({
  selector: 'app-automobile-list',
  standalone: true,
  imports: [FormsModule, CommonModule, MatPaginatorModule],
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
  totalItems = 0;
  page = 1;
  pageSize = 5;
  genres: string[] = ['Tourisme','Utilitaire','Camion','Bus / Minibus','Deux-roues','Engin agricole',
    'Engin de chantier','Remorque','Autre'];

  types: EnginType[] = [];
  filters: any = {
    matricule: '',
    genre: '',
    type: '',
    dateRange: ''
  };
  constructor(private autoService: AutoService, private toastr: ToastrService, private paginatorIntl: MatPaginatorIntl, private enginTypeService: EnginTypeService) {
    this.paginatorIntl.itemsPerPageLabel = 'Éléments par page';
    this.paginatorIntl.nextPageLabel = 'Suivant';
    this.paginatorIntl.previousPageLabel = 'Précédent';
    this.paginatorIntl.firstPageLabel = 'Premier';
    this.paginatorIntl.lastPageLabel = 'Dernier';
}

  ngOnInit(): void {
    this.loadAutos();
    this.loadTypes();
  }
  loadTypes() {
    this.enginTypeService.getAll().subscribe(data => {
      this.types = data;
    });
  }
  refreshList(): void {
    this.loadAutos();
  }
  resetFilters(): void {
    this.filters = {};
    this.searchText = '';
    this.loadAutos();
  }

  loadAutos() {
    const allFilters = { ...this.filters, page: this.page, pageSize: this.pageSize };
    this.autoService.getAutos(allFilters).subscribe((data: PagedResult<Auto>) => {
      this.autos = data.data;
      this.filteredAutos = this.autos;
      this.totalItems = data.totalItems; 
    });
  }


  onFilterChange(field: string, value: string) {
    this.filters[field] = value;
    this.loadAutos();
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadAutos();
  }
  search(): void {
    this.page = 1;
    this.filters['matricule'] = this.searchText.trim();
    this.loadAutos();
  }


  resetForm(): Auto {
    return {
      matricule: '',
      genre: '', enginTypeCode: '', marque: '', model: '',
      modeCarburant: '', acquisition: '',
      miseCirculationDate: new Date().toISOString(),
      etat: '', th: 0, tj: 0
    };
  }

  openAddModal(): void {
    this.autoForm = this.resetForm();
    this.editingAuto = null;
    this.showModal = true;
  }

  openEditModal(auto: Auto): void {
    this.editingAuto = auto;
    this.autoForm = { ...auto };
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.editingAuto = null;
  }

  validateForm(): boolean {
    this.errors = {};

    if (!this.autoForm.matricule?.trim()) this.errors.matricule = 'Le matricule est obligatoire';
    if (!this.autoForm.genre?.trim()) this.errors.genre = 'Le genre est obligatoire';
    if (!this.autoForm.enginTypeCode?.trim()) this.errors.type = 'Le type est obligatoire';
    if (!this.autoForm.marque?.trim()) this.errors.marque = 'La marque est obligatoire';
    if (!this.autoForm.modeCarburant?.trim()) this.errors.modeCarburant = 'Le mode de carburant est obligatoire';
    if (!this.autoForm.acquisition?.trim()) this.errors.acquisition = 'L\'acquisition est obligatoire';
    if (!this.autoForm.miseCirculationDate) this.errors.miseCirculationDate = 'La date de mise en service est obligatoire';
    if (!this.autoForm.etat?.trim()) this.errors.etat = 'L\'état du véhicule est obligatoire';

    return Object.keys(this.errors).length === 0;
  }

  saveAuto(): void {
    if (!this.validateForm()) {
      this.toastr.error('Veuillez remplir tous les champs obligatoires !', 'Formulaire invalide');
      return;
    }

    const selectedType = this.types.find(t => t.code === this.autoForm.enginTypeCode);
    if (selectedType) {
      this.autoForm.enginTypeName = selectedType.name;
      this.autoForm.enginTypeDescription = selectedType.description;
    }

    if (this.editingAuto) {
      this.autoService.updateAuto(this.autoForm).subscribe({
        next: () => {
          const index = this.autos.findIndex(a => a.id === this.autoForm.id);
          if (index !== -1) {this.autos[index] = { ...this.autoForm }; 
          }
          this.filteredAutos = [...this.autos];
          this.closeModal();
          this.toastr.success('Véhicule mis à jour !', 'Succès');
        },
        error: () => this.toastr.error('Erreur lors de la mise à jour', 'Erreur')
      });
    } else {
      this.autoService.addAuto(this.autoForm).subscribe({
        next: (res) => {
          res.enginTypeName = this.autoForm.enginTypeName;
          res.enginTypeDescription = this.autoForm.enginTypeDescription;
          this.autos.push(res);
          this.filteredAutos = [...this.autos];
          this.closeModal();
          this.toastr.success('Véhicule ajouté !', 'Succès');
        },
        error: () => this.toastr.error('Erreur lors de l\'ajout', 'Erreur')
      });
    }
  }


  deleteAuto(id: string): void {
    this.autoService.deleteAuto(id).subscribe({
      next: () => {
        this.autos = this.autos.filter(a => a.id !== id);
        this.filteredAutos = [...this.autos];
        this.toastr.success('Véhicule supprimé avec succès.', 'Supprimé !');
      },
      error: () => this.toastr.error('Erreur lors de la suppression du véhicule', 'Erreur')
    });
  }


  exportToWord() {
    this.autoService.getAll().subscribe({
      next: (engins: Auto[]) => {
        this.generateWordDoc(engins);
      },
      error: (err) => this.toastr.error('Erreur lors du chargement des véhicules', 'Erreur')
    });
  }

  private generateWordDoc(engins: Auto[]) {
    const tableRows: TableRow[] = [];

    const headers = [
      "N°", "Matricule", "Genre", "Type", "Marque",
      "Mode de Carburant", "Acquisition", "Date Mise en service",
      "État de véhicule"
    ];

    tableRows.push(new TableRow({
      children: headers.map(h =>
        new TableCell({
          children: [new Paragraph({ text: h })]
        })
      )
    }));

    engins.forEach((v, i) => {
      tableRows.push(new TableRow({
        children: [
          new TableCell({ children: [new Paragraph(String(i + 1))] }),
          new TableCell({ children: [new Paragraph(v.matricule)] }),
          new TableCell({ children: [new Paragraph(v.genre)] }),
          new TableCell({ children: [new Paragraph(v.enginTypeCode)] }),
          new TableCell({ children: [new Paragraph(v.marque)] }),
          new TableCell({ children: [new Paragraph(v.modeCarburant)] }),
          new TableCell({ children: [new Paragraph(v.acquisition)] }),
          new TableCell({ children: [new Paragraph(new Date(v.miseCirculationDate).toLocaleDateString())] }),
          new TableCell({ children: [new Paragraph(v.etat)] }),
        ]
      }));
    });


    const headerParagraphs = [
      new Paragraph({
        alignment: "left",
        children: [new TextRun({ text: "Royaume du Maroc", bold: true })],
      }),
      new Paragraph({
        alignment: "left",
        children: [new TextRun({ text: "Ministère de l’Intérieur", bold: true })],
      }),
      new Paragraph({
        alignment: "left",
        children: [new TextRun({ text: "Province de Khouribga", bold: true })],
      }),
      new Paragraph({
        alignment: "left",
        children: [new TextRun({ text: "Cercle de Khouribga", bold: true })],
      }),
      new Paragraph({
        alignment: "left",
        children: [new TextRun({ text: "Caïdat Ouled Abdoune", bold: true })],
      }),
      new Paragraph({
        alignment: "left",
        spacing: { after: 300 },
        children: [new TextRun({ text: "Commune Ouled Abdoune", bold: true })],
      }),

      new Paragraph({
        alignment: "center",
        spacing: { after: 400 },
        children: [
          new TextRun({
            text: "le parc automobile de la commune Ouled Abdoune ",
            bold: true,
            underline: {},   
            size: 36,
          })
        ]
      })
    ];

    const doc = new Document({
      sections: [{
        children: [
          ...headerParagraphs,
          new Table({
            width: { size: 100, type: WidthType.PERCENTAGE },
            rows: tableRows
          })
        ]
      }]
    });

    Packer.toBlob(doc).then(blob => {
      saveAs(blob, 'Liste_Vehicules.docx');
      this.toastr.success('Export Word effectué avec succès !', 'Export Word');
    });
  }


}
