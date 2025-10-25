import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { SidebarComponent } from '../../core/components/sidebar/sidebar.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, SidebarComponent,RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  sidebarOpen = false;
  hasActiveRoute = false;
 
  constructor(private router: Router) {}

  onToggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }
  onActivate() {
    setTimeout(() => this.hasActiveRoute = true);
  }

  onDeactivate() {
    setTimeout(() => this.hasActiveRoute = false);
  }

  onLogout(): void {
    // Pour l'instant, redirection vers login
    this.router.navigate(['/login']);
  }
}
