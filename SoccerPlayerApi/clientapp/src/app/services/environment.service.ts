import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { ResponseDto } from '../models/responseDto';
import { EnvironmentDto } from '../models/environments/environmentDto';
import { EnvironmentCreateDto } from '../models/environments/environmentCreateDto';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService {
  private url = `${environment.apiUrl}/Environment`;

  constructor(private httpClient: HttpClient) {}
      
  getEnvironments(): Observable<ResponseDto<EnvironmentDto[]>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<EnvironmentDto[]>>(`${this.url}/environments`, { headers : header });
  }

  getEnvironmentById(id: number): Observable<ResponseDto<EnvironmentDto>> {
    return this.httpClient.get<ResponseDto<EnvironmentDto>>(`${this.url}/environment/${id}`);
  }

  createEnvironment(environment: EnvironmentCreateDto): Observable<ResponseDto<number>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<number>>(`${this.url}/create`, environment, { headers : header });
  }
}
