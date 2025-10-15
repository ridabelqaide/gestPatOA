import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  showPassword = false;

  registerData = {
    username: '',
    email: '',
    password: '',
    roleId: 0
  };

  errorMessage?: string;
  successMessage?: string;

  constructor(private router: Router, private authService: AuthService) { }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    this.errorMessage = undefined;
    this.successMessage = undefined;

    if (!this.registerData.username || !this.registerData.email || !this.registerData.password) {
      this.errorMessage = 'Veuillez remplir tous les champs';
      return;
    }

    this.authService.register(this.registerData).subscribe({
      next: (res) => {
        this.successMessage = 'Inscription réussie ! Vous pouvez maintenant vous connecter.';
        console.log('Utilisateur créé :', res);
        this.router.navigate(['/home']);

      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Erreur lors de l\'inscription';
        console.error('Erreur register:', this.errorMessage);
      }
    });
  }
}
