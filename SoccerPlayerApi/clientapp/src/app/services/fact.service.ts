import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { FactUpdateDto } from '../models/facts/factUpdateDto';
import { ResponseDto } from '../models/responseDto';
import { DataTypeDto } from '../models/facts/DataTypeDto';
import { FactDto } from '../models/facts/FactDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { FactCreateDto } from '../models/facts/factCreateDto';

@Injectable({
  providedIn: 'root'
})
export class FactService {
  private apiUrl = `${environment.apiUrl}/fact`;

  constructor(private httpClient: HttpClient) {}

  getFacts(filter: GetFactFilterDto): Observable<ResponseDto<FactDto[]>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<FactDto[]>>(`${this.apiUrl}/facts`, filter, { headers : header })
  }

  createFact(fact: FactCreateDto): Observable<ResponseDto<number>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<number>>(`${this.apiUrl}/createfact`, fact, { headers : header })
  }

  updateFact(fact: FactUpdateDto): Observable<boolean> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<boolean>(`${this.apiUrl}/updatefact`, fact, { headers : header })
  }

  getFactTypes(): Observable<ResponseDto<DataTypeDto[]>> {
    return this.httpClient.get<ResponseDto<DataTypeDto[]>>(`${this.apiUrl}/facttypes`)
  }

  getDataTypes(): Observable<ResponseDto<DataTypeDto[]>> {
    return this.httpClient.get<ResponseDto<DataTypeDto[]>>(`${this.apiUrl}/types`)
  }

  createType(type: DataTypeDto): Observable<ResponseDto<DataTypeDto>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<DataTypeDto>>(`${this.apiUrl}/createtype`, type, { headers : header })
  }
}
