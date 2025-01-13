import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ScopeDto } from '../models/scopes/scopeDto';
import { ResponseDto } from '../models/responseDto';
import { ScopeFilterDto } from '../models/scopes/scopeFilterDto';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {

  private url = `${environment.apiUrl}/history`;

  constructor(private http: HttpClient) {}

  getScopes(filter: ScopeFilterDto): Observable<ResponseDto<ScopeDto[]>> {
      const header = new HttpHeaders().set('Content-type', 'application/json');
      return this.http.post<ResponseDto<ScopeDto[]>>(`${this.url}/scopes`, filter, { headers : header });
  }
}
