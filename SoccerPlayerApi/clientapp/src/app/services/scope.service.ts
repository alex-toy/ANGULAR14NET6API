import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ScopeDto } from '../models/scopes/scopeDto';
import { ResponseDto } from '../models/responseDto';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';

@Injectable({
  providedIn: 'root'
})
export class ScopeService {
  private url = `${environment.apiUrl}/scope`;

  constructor(private httpClient: HttpClient) {}
  
  getScopeData(scope: ScopeDto): Observable<ResponseDto<GetScopeDataDto[]>> {
      const header = new HttpHeaders().set('Content-type', 'application/json');
      return this.httpClient.post<ResponseDto<GetScopeDataDto[]>>(`${this.url}/scopes`, scope, { headers : header });
  }
}
