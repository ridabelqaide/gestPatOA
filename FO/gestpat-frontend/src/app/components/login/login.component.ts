import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';
import { LoginRequest } from '../../models/auth.model';
import { HttpClientModule } from '@angular/common/http'; 

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  showPassword = false;

  loginData = {
    username: '',
    password: '',
    rememberMe: false
  };

  errorMessage?: string;

  constructor(private router: Router, private authService: AuthService) { }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {

    if (!this.loginData.username || !this.loginData.password) {
      this.errorMessage = 'Veuillez remplir tous les champs';
      return;
    }

    const loginRequest: LoginRequest = {
      username: this.loginData.username,
      password: this.loginData.password
    };

    this.authService.login(loginRequest).subscribe({
      next: (res) => {
        if (!res) {
          this.errorMessage = 'username ou mot de passe incorrect';
          return;
        }

        console.log('Données de connexion:', this.loginData);
        this.router.navigate(['/home']);
      },
     
    });

  }
}
