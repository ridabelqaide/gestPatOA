import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../Environments/environment';
import { Official } from '../models/Official.model';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OfficialService {

  private baseUrl = `${environment.base_url}/api/Official`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Official[]> {
    return this.http.get<Official[]>(this.baseUrl);
  }


  getPaged(genre?: string, fonction?: string, service?: string, page = 1, pageSize = 5): Observable<any> {
    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (genre) params = params.set('genre', genre);
    if (fonction) params = params.set('fonction', fonction);
    if (service) params = params.set('service', service);

    return this.http.get<any>(`${this.baseUrl}/paged`, { params, headers });
  }

  getById(id: string): Observable<Official> {
    return this.http.get<Official>(`${this.baseUrl}/${id}`);
  }

  create(data: Official): Observable<any> {
    return this.http.post(this.baseUrl, data);
  }

  update(id: string, data: Official): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
