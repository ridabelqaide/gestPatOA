import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Auto } from '../models/auto.model';
import { environment } from '../../Environments/environment';
import { PagedResult } from '../models/paged-result.model';
@Injectable({
  providedIn: 'root'
})
export class AutoService {
  private API_URL = `${environment.base_url}/api/engins`; 
  constructor(private http: HttpClient) { }

  getAutos(filters: any) {
    let params = new HttpParams();
    Object.keys(filters).forEach(key => {
      if (filters[key] !== null && filters[key] !== '') {
        params = params.append(key, filters[key]);
      }
    });
    return this.http.get<PagedResult<Auto>>(`${this.API_URL}`, { params });
  }

  getAll(): Observable<Auto[]> {
    return this.http.get<Auto[]>(`${this.API_URL}/all`);
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
  getLastInsurance(filters?: any) {
    const params = new URLSearchParams();

    if (filters) {
      Object.keys(filters).forEach(key => {
        if (filters[key] !== '' && filters[key] !== null && filters[key] !== undefined) {
          params.append(key, filters[key]);
        }
      });
    }

    return this.http.get<any[]>(`${this.API_URL}/last-insurance?${params.toString()}`);
  }
  getLastInsuranceAll(): Observable<Auto[]> {
    return this.http.get<Auto[]>(`${this.API_URL}/lastInsuranceNPaged`);
  }

}
