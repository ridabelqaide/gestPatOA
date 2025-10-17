import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Auto } from '../models/auto.model';
import { environment } from '../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutoService {
  private API_URL = `${environment.base_url}/api/engins`; 
  constructor(private http: HttpClient) { }

  getAutos(): Observable<Auto[]> {
    return this.http.get<Auto[]>(this.API_URL);
  }

  addAuto(auto: Auto): Observable<Auto> {
    return this.http.post<Auto>(this.API_URL, auto);
  }

  updateAuto(auto: Auto): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${auto.id}`, auto);
  }

  deleteAuto(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
