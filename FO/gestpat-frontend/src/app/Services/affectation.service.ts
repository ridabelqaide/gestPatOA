import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Affectation } from '../models/Affectation';
import { environment } from '../../Environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AffectationService {

  private API_URL = `${environment.base_url}/api/Affectation`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Affectation[]> {
    return this.http.get<Affectation[]>(this.API_URL);
  }
  getPaged(page: number, pageSize: number, officialId?: string, enginId?: string, startDate?: string): Observable<any> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (officialId) params = params.set('officialId', officialId);
    if (enginId) params = params.set('enginId', enginId);
    if (startDate) params = params.set('startDate', startDate);

    return this.http.get<any>(`${this.API_URL}/paged`, { params });
  }


  getById(id: string): Observable<Affectation> {
    return this.http.get<Affectation>(`${this.API_URL}/${id}`);
  }

  create(affectation: Affectation): Observable<Affectation> {
    return this.http.post<Affectation>(this.API_URL, affectation);
  }

  update(id: string, affectation: Affectation): Observable<Affectation> {
    return this.http.put<Affectation>(`${this.API_URL}/${id}`, affectation);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
