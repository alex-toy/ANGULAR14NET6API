import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { ResponseDto } from '../models/responseDto';
import { EnvironmentDto } from '../models/environments/environmentDto';
import { FrameCreateDto } from '../models/environments/environmentCreateDto';
import { EnvironmentUpdateDto } from '../models/environments/environmentUpdateDto';
import { EnvironmentSortingDto } from '../models/environments/environmentSortingDto';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService {
  private apiUrl = `${environment.apiUrl}/Frame`;

  constructor(private httpClient: HttpClient) {}
      
  getEnvironments(): Observable<ResponseDto<EnvironmentDto[]>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<EnvironmentDto[]>>(`${this.apiUrl}/frames`, { headers : header });
  }

  getEnvironmentById(id: number): Observable<ResponseDto<EnvironmentDto>> {
    return this.httpClient.get<ResponseDto<EnvironmentDto>>(`${this.apiUrl}/frame/${id}`);
  }

  createEnvironment(environment: FrameCreateDto): Observable<ResponseDto<number>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<ResponseDto<number>>(`${this.apiUrl}/create`, environment, { headers : header });
  }

  updateEnvironment(environment: EnvironmentUpdateDto): Observable<ResponseDto<number>> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.put<ResponseDto<number>>(`${this.apiUrl}/update`, environment, { headers : header });
  }

  deleteEnvironment(id: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.apiUrl}/delete/${id}`);
  }
  
  createEnvironmentSorting(sortingDto: EnvironmentSortingDto): Observable<any> {
    return this.httpClient.post<any>(`${this.apiUrl}/create-environment-sorting`, sortingDto);
  }
}
