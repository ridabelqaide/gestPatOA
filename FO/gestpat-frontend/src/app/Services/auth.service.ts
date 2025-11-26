import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, RegisterRequest } from '../models/auth.model';
import { environment } from '../../Environments/environment';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private API_URL = `${environment.base_url}/api/auth`;
  private API_URL_ADMIN = `${environment.base_url}/api/admin`;
  constructor(private http: HttpClient) { }

  
  login(data: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.API_URL}/login`, data).pipe(
      tap(res => {
        localStorage.setItem('token', res.token!);
        const user = {
          username: res.username,   
          email: res.email,        
          role: res.role        
        };
        console.log('User logged in:', user);
        localStorage.setItem('user', JSON.stringify(user));
      })
    );
  }

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/register`, data);
  }
  getRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.API_URL_ADMIN}/roles`);
  }
  getRole(): string {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    return user.role?.toLowerCase() ?? '';
  }

  isAdmin(): boolean {
    return this.getRole() === 'admin';
  }


  logout(): void {
    const token = localStorage.getItem('token');

    if (token) {
      this.http.post(
        `${this.API_URL}/logout`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      ).subscribe({
        next: () => console.log("Backend logout success"),
        error: (err) => console.error("Backend logout error", err)
      });
    }

    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }


}
