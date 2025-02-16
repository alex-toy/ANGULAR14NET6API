import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AlgorithmDto } from '../models/forecasts/algorithms/algorithmDto';
import { ResponseDto } from '../models/responseDto';

@Injectable({
  providedIn: 'root'
})
export class SimulationService {
  private apiUrl = `${environment.apiUrl}/simulation`;

  constructor(private httpClient: HttpClient) {}
  
    getAlgorithms(): Observable<ResponseDto<AlgorithmDto[]>> {
      return this.httpClient.get<ResponseDto<AlgorithmDto[]>>(`${this.apiUrl}/algorithms`)
    }
}
