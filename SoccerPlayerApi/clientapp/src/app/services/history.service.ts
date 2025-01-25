import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ScopeDto } from '../models/scopes/scopeDto';
import { ResponseDto } from '../models/responseDto';
import { ScopeFilterDto } from '../models/scopes/scopeFilterDto';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';
import { TimeAggregationDto } from '../models/facts/timeAggregationDto';

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

  getScopesByEnvironment(environmentId: number): Observable<ResponseDto<ScopeDto[]>> {
      return this.http.get<ResponseDto<ScopeDto[]>>(`${this.url}/scopesbyenvironmentid/${environmentId}`)
  }
    
  getScopeData(scope: ScopeDto): Observable<ResponseDto<GetScopeDataDto[]>> {
      const header = new HttpHeaders().set('Content-type', 'application/json');
      return this.http.post<ResponseDto<GetScopeDataDto[]>>(`${this.url}/data`, scope, { headers : header });
  }
    
  getTimeAggregations(levelId: number): Observable<ResponseDto<TimeAggregationDto[]>> {
      return this.http.get<ResponseDto<TimeAggregationDto[]>>(`${this.url}/timeaggregations/${levelId}`)
  }
}
