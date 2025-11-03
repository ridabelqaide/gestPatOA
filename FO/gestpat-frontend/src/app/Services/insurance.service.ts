import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Insurance } from '../models/insurance.model';
import { environment } from '../../Environments/environment';
@Injectable({ providedIn: 'root' })
export class InsuranceService {
  private API_URL = `${environment.base_url}/api/insurances`; 

  constructor(private http: HttpClient) { }

  getAll(): Observable<Insurance[]> {
    return this.http.get<Insurance[]>(this.API_URL);
  }

  create(insurance: Insurance): Observable<Insurance> {
    return this.http.post<Insurance>(this.API_URL, insurance);
  }

  update(id: string, insurance: Insurance): Observable<Insurance> {
    return this.http.put<Insurance>(`${this.API_URL}/${id}`, insurance);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
