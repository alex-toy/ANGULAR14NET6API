import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { FactCreateDto } from '../models/facts/factCreateDto';
import { FactCreateResultDto } from '../models/facts/factCreateResultDto';
import { FactUpdateDto } from '../models/facts/factUpdateDto';
import { ResponseDto } from '../models/responseDto';
import { TypeDto } from '../models/facts/typeDto';
import { AggregationCreateDto } from '../models/aggregations/aggregationCreateDto';

@Injectable({
  providedIn: 'root'
})
export class FactService {
  private apiUrl = `${environment.apiUrl}/fact`;

  constructor(private httpClient: HttpClient) {}

  getFacts(filter: GetFactFilterDto): Observable<GetFactsResultDto> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<GetFactsResultDto>(`${this.apiUrl}/facts`, filter, { headers : header })
  }

  createFact(fact: FactCreateDto): Observable<FactCreateResultDto> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<FactCreateResultDto>(`${this.apiUrl}/createfact`, fact, { headers : header })
  }

  updateFact(fact: FactUpdateDto): Observable<boolean> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<boolean>(`${this.apiUrl}/updatefact`, fact, { headers : header })
  }

  getFactTypes(): Observable<ResponseDto<TypeDto[]>> {
    return this.httpClient.get<ResponseDto<TypeDto[]>>(`${this.apiUrl}/facttypes`)
  }

  getTypes(): Observable<ResponseDto<TypeDto[]>> {
    return this.httpClient.get<ResponseDto<TypeDto[]>>(`${this.apiUrl}/types`)
  }

  createType(type: TypeDto): Observable<ResponseDto<TypeDto>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<TypeDto>>(`${this.apiUrl}/createtype`, type, { headers : header })
  }
}
