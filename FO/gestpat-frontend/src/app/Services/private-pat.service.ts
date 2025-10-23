import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PrivatePat } from '../models/private-pat.model';
import { environment } from '../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PrivatePatService {

  private API_URL = `${environment.base_url}/api/PrivatePat`;

  constructor(private http: HttpClient) { }

  getAll(registrationNumber?: string): Observable<PrivatePat[]> {
    let params = new HttpParams();
    if (registrationNumber) {
      params = params.set('registrationNumber', registrationNumber);
    }
    return this.http.get<PrivatePat[]>(this.API_URL, { params });
  }


  getById(id: string): Observable<PrivatePat> {
    return this.http.get<PrivatePat>(`${this.API_URL}/${id}`);
  }

  create(data: PrivatePat): Observable<PrivatePat> {
    return this.http.post<PrivatePat>(this.API_URL, data);
  }

  update(id: string, data: PrivatePat): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, data);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
