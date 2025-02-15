import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ScopeDto } from '../models/scopes/scopeDto';
import { ResponseDto } from '../models/responseDto';
import { ScopeFilterDto } from '../models/scopes/scopeFilterDto';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';
import { TimeAggregationDto } from '../models/facts/timeAggregationDto';
import { EnvironmentScopeDto } from '../models/scopes/environmentScopeDto';
import { scopeByDataTypeDto } from '../models/scopes/scopeByDataTypeDto';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {

  private url = `${environment.apiUrl}/history`;

  constructor(private http: HttpClient) {}

  getScopes(filter: ScopeFilterDto): Observable<ResponseDto<EnvironmentScopeDto[]>> {
      const header = new HttpHeaders().set('Content-type', 'application/json');
      return this.http.post<ResponseDto<EnvironmentScopeDto[]>>(`${this.url}/scopes`, filter, { headers : header });
  }

  getScopesByEnvironment(environmentId: number): Observable<ResponseDto<EnvironmentScopeDto[]>> {
      return this.http.get<ResponseDto<EnvironmentScopeDto[]>>(`${this.url}/scopesbyenvironmentid/${environmentId}`)
  }
    
  getScopeData(scope: EnvironmentScopeDto): Observable<ResponseDto<GetScopeDataDto[]>> {
      const header = new HttpHeaders().set('Content-type', 'application/json');
      return this.http.post<ResponseDto<GetScopeDataDto[]>>(`${this.url}/data`, scope, { headers : header });
  }
    
  getScopeDataTest(scope: EnvironmentScopeDto) : Observable<ResponseDto<scopeByDataTypeDto>> {
      const header = new HttpHeaders().set('Content-type', 'application/json');
      return this.http.post<ResponseDto<scopeByDataTypeDto>>(`${this.url}/scopedata`, scope, { headers : header });
  }
    
  getTimeAggregations(levelId: number): Observable<ResponseDto<TimeAggregationDto[]>> {
      return this.http.get<ResponseDto<TimeAggregationDto[]>>(`${this.url}/timeaggregations/${levelId}`)
  }
}
