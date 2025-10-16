import { Component, OnInit } from '@angular/core';
import { AutoService } from '../../Services/auto-list.service';
import { Auto } from '../../models/auto.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
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
      error: (err) => console.error(err)
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

  saveAuto() {
    console.log('Données envoyées au backend :', JSON.stringify(this.autoForm)); // ← Ajouté

    if (this.editingAuto) {
      this.autoService.updateAuto(this.autoForm).subscribe({
        next: () => {
          const index = this.autos.findIndex(a => a.id === this.autoForm.id);
          if (index !== -1) this.autos[index] = { ...this.autoForm };
          this.filteredAutos = [...this.autos];
          this.closeModal();
        },
        error: (err) => console.error('Erreur updateAuto :', err)
      });
    } else {
      this.autoService.addAuto(this.autoForm).subscribe({
        next: (res) => {
          this.autos.push(res);
          this.filteredAutos = [...this.autos];
          this.closeModal();
        },
        error: (err) => console.error('Erreur addAuto :', err)
      });
    }
  }


  deleteAuto(id: string) {
    if (confirm('Voulez-vous vraiment supprimer cet auto ?')) {
      this.autoService.deleteAuto(id).subscribe({
        next: () => {
          this.autos = this.autos.filter(a => a.id !== id);
          this.filteredAutos = [...this.autos];
        }
      });
    }
  }

  search() {
    const text = this.searchText.toLowerCase();
    this.filteredAutos = this.autos.filter(a =>
      a.matricule.toLowerCase().includes(text) ||
      a.marque.toLowerCase().includes(text) ||
      a.model.toLowerCase().includes(text)
    );
  }
}
