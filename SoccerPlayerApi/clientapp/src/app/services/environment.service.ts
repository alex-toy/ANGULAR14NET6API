import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { ResponseDto } from '../models/responseDto';
import { EnvironmentDto } from '../models/environments/environmentDto';
import { EnvironmentCreateDto } from '../models/environments/environmentCreateDto';
import { EnvironmentUpdateDto } from '../models/environments/environmentUpdateDto';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService {
  private apiUrl = `${environment.apiUrl}/Environment`;

  constructor(private http: HttpClient) {}
      
  getEnvironments(): Observable<ResponseDto<EnvironmentDto[]>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.post<ResponseDto<EnvironmentDto[]>>(`${this.apiUrl}/environments`, { headers : header });
  }

  getEnvironmentById(id: number): Observable<ResponseDto<EnvironmentDto>> {
    return this.http.get<ResponseDto<EnvironmentDto>>(`${this.apiUrl}/environment/${id}`);
  }

  createEnvironment(environment: EnvironmentCreateDto): Observable<ResponseDto<number>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.post<ResponseDto<number>>(`${this.apiUrl}/create`, environment, { headers : header });
  }

  updateEnvironment(environment: EnvironmentUpdateDto): Observable<any> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.http.put<any>(`${this.apiUrl}/update`, environment, { headers : header });
  }

  deleteEnvironment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete/${id}`);
  }
}
