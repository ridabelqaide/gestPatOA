import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PublicPat } from '../models/public-pat.model';
import { environment } from '../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PublicPatService {
  private API_URL = `${environment.base_url}/api/PublicPat`; 

  constructor(private http: HttpClient) { }

  getAll(registrationNumber?: string): Observable<PublicPat[]> {
    let params = new HttpParams();
    if (registrationNumber) {
      params = params.set('registrationNumber', registrationNumber);
    }
    return this.http.get<PublicPat[]>(this.API_URL, { params });
  }

  getById(id: string): Observable<PublicPat> {
    return this.http.get<PublicPat>(`${this.API_URL}/${id}`);
  }

  create(data: PublicPat): Observable<PublicPat> {
    return this.http.post<PublicPat>(this.API_URL, data);
  }

  update(id: string, data: PublicPat): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, data);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
