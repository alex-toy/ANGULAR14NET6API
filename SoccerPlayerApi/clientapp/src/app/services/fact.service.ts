import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';

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
}
