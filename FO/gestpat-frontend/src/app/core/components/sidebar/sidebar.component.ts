import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from '../../../Services/auth.service';

interface MenuItem {
  id: string;
  label: string;
  icon: string;
  subMenus?: SubMenuItem[];
  isOpen?: boolean;
}

interface SubMenuItem {
  id: string;
  label: string;
  icon: string;
  route?: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  @Input() isOpen = false;
  @Output() toggleSidebar = new EventEmitter<void>();
 // @Output() logout = new EventEmitter<void>();

  activeRoute = '';

  menuItems: MenuItem[] = [
    {
      id: 'Dashboard',
      label: 'Dashboard',
      icon: 'M3 4a1 1 0 011-1h12a1 1 0 011 1v2a1 1 0 01-1 1H4a1 1 0 01-1-1V4zM3 10a1 1 0 011-1h6a1 1 0 011 1v6a1 1 0 01-1 1H4a1 1 0 01-1-1v-6zM14 9a1 1 0 00-1 1v6a1 1 0 001 1h2a1 1 0 001-1v-6a1 1 0 00-1-1h-2z',
    },
    {
      id: 'PatPublic',
      label: 'Patrimoine public',
      icon: 'M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4',
      subMenus: [
        { id: 'propriétéCollective', route: '/home/PublicPat', label: 'Propriété Collective', icon: 'M3 12l6-6 6 6 6-6v10a2 2 0 01-2 2H5a2 2 0 01-2-2V12z' },
        { id: 'Souk', route: 'Souk', label: 'Souk Hebdomadaire', icon: 'M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z M15 11a3 3 0 11-6 0 3 3 0 016 0z' },
        { id: 'Cimetières', route: 'Cimetières', label: 'Cimetières', icon: 'M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z' },
        { id: 'Lotissments', route: 'Lotissments', label: 'Lotissments', icon: 'M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4' },

      ]
    },
    {
      id: 'PatPrive',
      label: 'Patrimoine privé',
      icon: 'M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2H5a2 2 0 00-2-2z M8 5a2 2 0 012-2h4a2 2 0 012 2v2H8V5z',
      subMenus: [
        { id: 'propriétéCollective', route: '/home/PrivatePat', label: 'Propriété Collective', icon: 'M3 12l6-6 6 6 6-6v10a2 2 0 01-2 2H5a2 2 0 01-2-2V12z' },
        { id: 'TerrainsBatis', route: 'TerrainsBatis', label: 'Terrains bâtis', icon: 'M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4' },
        { id: 'TerrainsNonBatis', route: 'TerrainsNonBatis', label: 'Terrains non bâtis', icon: 'M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z M15 11a3 3 0 11-6 0 3 3 0 016 0z' },
        { id: 'Boutiques', route: 'Boutiques', label: 'Boutiques Commerciales', icon: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z' },
        { id: 'Pharmacies', route: 'Pharmacies', label: 'Pharmacies', icon: 'M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z' },
        { id: 'Logements', route: 'Logements', label: 'Logements', icon: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6' },
      ]
    },
    {
      id: 'ParcAuto',
      label: 'Parc Automobile',
      icon: 'M8 7v8a2 2 0 002 2h6M8 7V5a2 2 0 012-2h4.586a1 1 0 01.707.293l4.414 4.414a1 1 0 01.293.707V15a2 2 0 01-2 2h-2M8 7H6a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2v-2',
      subMenus: [
        { id: 'ListAuto', route: '/home/autoList', label: 'List Automobile', icon: 'M4 6h16M4 10h16M4 14h16M4 18h16' },
        { id: 'TypeAuto', route: '/home/EnginType', label: 'Type Auto', icon: 'M8 7v8a2 2 0 002 2h6M8 7V5a2 2 0 012-2h4.586a1 1 0 01.707.293l4.414 4.414a1 1 0 01.293.707V15a2 2 0 01-2 2h-2M8 7H6a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2v-2' },
        { id: 'Assurances', route: 'home/ListAutoInsurance', label: 'Assurances', icon: 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z' },
        { id: 'VisitesTechnique', route: 'VisitesTechnique', label: 'Visites Technique', icon: 'M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01' },
        { id: 'Affectation', route: 'Affectation', label: 'Affectation', icon: 'M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z' },
        { id: 'Maintenance', route: 'Maintenance', label: 'Maintenance', icon: 'M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z M15 12a3 3 0 11-6 0 3 3 0 016 0z' },
      ]
    },
    {
      id: 'Admin',
      label: 'Admin Actions',
      icon: 'M12 21v-8.25M15.75 21v-8.25M8.25 21v-8.25M3 9l9-6 9 6m-1.5 12V10.332A48.36 48.36 0 0 0 12 9.75c-2.551 0-5.056.2-7.5.582V21M3 21h18M12 6.75h.008v.008H12V6.75Z',
      subMenus: [
        { id: 'Fonctionnaire', route: '/home/Official', label: 'Les Fonctionnaires', icon: 'M18 18.72a9.094 9.094 0 0 0 3.741-.479 3 3 0 0 0-4.682-2.72m.94 3.198.001.031c0 .225-.012.447-.037.666A11.944 11.944 0 0 1 12 21c-2.17 0-4.207-.576-5.963-1.584A6.062 6.062 0 0 1 6 18.719m12 0a5.971 5.971 0 0 0-.941-3.197m0 0A5.995 5.995 0 0 0 12 12.75a5.995 5.995 0 0 0-5.058 2.772m0 0a3 3 0 0 0-4.681 2.72 8.986 8.986 0 0 0 3.74.477m.94-3.197a5.971 5.971 0 0 0-.94 3.197M15 6.75a3 3 0 1 1-6 0 3 3 0 0 1 6 0Zm6 3a2.25 2.25 0 1 1-4.5 0 2.25 2.25 0 0 1 4.5 0Zm-13.5 0a2.25 2.25 0 1 1-4.5 0 2.25 2.25 0 0 1 4.5 0Z' },
        
      ]
    },
  ];

  constructor(private router: Router, private authService: AuthService) {
    // Écouter les changements de route pour mettre à jour l'état actif
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.activeRoute = event.url;
      });
  }
  
  onToggleSidebar(): void {
    this.toggleSidebar.emit();
  }

  //onLogout(): void {
  //  this.logout.emit();
  //}
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
  get isAdmin(): boolean {
    const isAdmin = this.authService.isAdmin();
    return isAdmin;
  }

  toggleSubMenu(menuItem: MenuItem): void {
    if (menuItem.subMenus) {
      menuItem.isOpen = !menuItem.isOpen;
    }
  }

  navigateToRoute(route?: string): void {
    if (route) {
      this.router.navigate([route]);
    }
  }

  isActiveRoute(route?: string): boolean {
    if (!route) return false;
    return this.activeRoute === `/${route}` || this.activeRoute === route;
  }

  isMenuActive(menuItem: MenuItem): boolean {
    // Vérifier si le menu principal est actif (Dashboard)
    if (menuItem.id === 'Dashboard' && this.activeRoute === '/home') {
      return true;
    }
    
    // Vérifier si un sous-menu est actif
    if (menuItem.subMenus) {
      return menuItem.subMenus.some(subMenu => this.isActiveRoute(subMenu.route));
    }
    
    return false;
  }
}
