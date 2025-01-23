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

@Injectable({
  providedIn: 'root'
})
export class FactService {
  private url = `${environment.apiUrl}/fact`;

  constructor(private httpClient: HttpClient) {}

  getFacts(filter: GetFactFilterDto): Observable<GetFactsResultDto> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<GetFactsResultDto>(`${this.url}/facts`, filter, { headers : header })
  }

  getFactTypes(): Observable<ResponseDto<string[]>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.get<ResponseDto<string[]>>(`${this.url}/facttypes`)
  }

  createFact(fact: FactCreateDto): Observable<FactCreateResultDto> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<FactCreateResultDto>(`${this.url}/createfact`, fact, { headers : header })
  }

  updateFact(fact: FactUpdateDto): Observable<boolean> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<boolean>(`${this.url}/updatefact`, fact, { headers : header })
  }
}
