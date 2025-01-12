import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ScopeDto } from '../models/scopes/scopeDto';
import { ResponseDto } from '../models/responseDto';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {

  private url = `${environment.apiUrl}/history`;

  constructor(private http: HttpClient) {}

  getScopes(): Observable<ResponseDto<ScopeDto[]>> {
    return this.http.get<ResponseDto<ScopeDto[]>>(`${this.url}/scopes`);
  }
}
