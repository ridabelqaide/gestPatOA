import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  showPassword = false;
  
  loginData = {
    email: '',
    password: '',
    rememberMe: false
  };

  constructor(private router: Router) {}

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.loginData.email && this.loginData.password) {
      console.log('Données de connexion:', this.loginData);
      // Pour l'instant, redirection directe vers home sans API
      // Ici vous pouvez ajouter la logique d'authentification
      // Par exemple, appeler un service d'authentification
      this.router.navigate(['/home']);
    }
  }
}
