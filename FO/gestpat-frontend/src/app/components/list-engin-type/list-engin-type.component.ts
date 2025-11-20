import { Component } from '@angular/core';
import { EnginType } from '../../models/engin-type.model';
import { EnginTypeService } from '../../Services/engin-type.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatPaginatorModule, PageEvent, MatPaginatorIntl } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { Document, Packer, Paragraph, Table, TableRow, TableCell, TextRun, WidthType } from 'docx';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-list-engin-type',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
  templateUrl: './list-engin-type.component.html',
  styleUrls: ['./list-engin-type.component.css']
})
export class ListEnginTypeComponent {

  types: EnginType[] = [];
  searchText: string = '';
  totalItems = 0;
  page = 1;
  pageSize = 5;

  showAddForm: boolean = false;
  showEditForm: boolean = false;

  newType: EnginType = { code: '', name: '', description: '' };
  editingType: EnginType | null = null;
  fields: (keyof EnginType)[] = ['code', 'name', 'description'];
  constructor(
    private service: EnginTypeService,
    private paginatorIntl: MatPaginatorIntl,
    private toastr: ToastrService
  ) {
    this.paginatorIntl.itemsPerPageLabel = 'Éléments par page';
    this.paginatorIntl.nextPageLabel = 'Suivant';
    this.paginatorIntl.previousPageLabel = 'Précédent';
    this.paginatorIntl.firstPageLabel = 'Premier';
    this.paginatorIntl.lastPageLabel = 'Dernier';
  }

  ngOnInit(): void {
    this.loadTypes();
  }

  get currentType(): EnginType {
    return this.showAddForm ? this.newType : (this.editingType as EnginType);
  }

  loadTypes(): void {
    this.service.search(this.searchText, this.page, this.pageSize)
      .subscribe({
        next: res => {
          this.types = res.data;
          this.totalItems = res.totalItems;
        },
        error: () => this.toastr.error('Erreur lors du chargement', 'Erreur')
      });
  }

  onSearch(): void {
    this.page = 1;
    this.loadTypes();
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadTypes();
  }

  toggleAddForm(): void {
    this.showAddForm = !this.showAddForm;
    this.showEditForm = false;
    this.newType = { code: '', name: '', description: '' };
  }

  createType(): void {
    if (!this.newType.code || !this.newType.name) {
      this.toastr.warning('Code et Name sont obligatoires');
      return;
    }
    this.service.create(this.newType).subscribe({
      next: () => {
        this.toastr.success('Type ajouté avec succès !');
        this.loadTypes();
        this.toggleAddForm();
      },
      error: () => this.toastr.error('Erreur lors de l\'ajout')
    });
  }

  editType(type: EnginType): void {
    this.editingType = { ...type };
    this.showEditForm = true;
    this.showAddForm = false;
  }

  saveEdit(): void {
    if (!this.editingType) return;

    this.service.update(this.editingType.code!, this.editingType).subscribe({
      next: () => {
        this.toastr.success('Type modifié avec succès !');
        this.loadTypes();
        this.cancelEdit();
      },
      error: () => this.toastr.error('Erreur lors de la modification')
    });
  }

  cancelEdit(): void {
    this.editingType = null;
    this.showEditForm = false;
  }

  deleteType(code: string): void {
    this.service.delete(code).subscribe({
      next: () => {
        this.toastr.success('Type supprimé !');
        this.loadTypes();
      },
      error: () => this.toastr.error('Erreur lors de la suppression')
    });
  }

  exportWord(): void {
    this.service.search('', 1, 9999).subscribe({
      next: res => this.generateWordDoc(res.data),
      error: () => this.toastr.error('Erreur lors de l\'export')
    });
  }

  private generateWordDoc(types: EnginType[]): void {
    const tableRows: TableRow[] = [
      new TableRow({
        children: ["Code", "Nom", "Description"].map(h =>
          new TableCell({ children: [new Paragraph({ children: [new TextRun(h)] })] })
        )
      })
    ];

    types.forEach(t => {
      tableRows.push(new TableRow({
        children: [
          new TableCell({ children: [new Paragraph(t.code || '')] }),
          new TableCell({ children: [new Paragraph(t.name || '')] }),
          new TableCell({ children: [new Paragraph(t.description || '')] }),
        ]
      }));
    });

    const doc = new Document({
      sections: [{
        children: [
          new Paragraph({ text: "Liste des types Auto", spacing: { after: 300 } }),
          new Table({ width: { size: 100, type: WidthType.PERCENTAGE }, rows: tableRows })
        ]
      }]
    });

    Packer.toBlob(doc).then(blob => {
      saveAs(blob, "EnginTypes.docx");
      this.toastr.success('Export Word réussi !');
    });
  }
}
