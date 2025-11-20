import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../Environments/environment';
import { EnginType } from '../models/engin-type.model';
import { PagedResult } from '../models/paged-result.model'; 

@Injectable({
  providedIn: 'root'
})
export class EnginTypeService {
  private API_URL = `${environment.base_url}/api/EnginType`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<EnginType[]> {
    return this.http.get<EnginType[]>(this.API_URL);
  }

  getByCode(code: string): Observable<EnginType> {
    return this.http.get<EnginType>(`${this.API_URL}/${code}`);
  }

  create(type: EnginType): Observable<EnginType> {
    return this.http.post<EnginType>(this.API_URL, type);
  }

  update(code: string, type: EnginType): Observable<EnginType> {
    return this.http.put<EnginType>(`${this.API_URL}/${code}`, type);
  }

  delete(code: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${code}`);
  }

  search(name: string = '', pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<EnginType>> {
    let params = new HttpParams()
      .set('name', name)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResult<EnginType>>(`${this.API_URL}/search`, { params });
  }
}
