import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AggregationCreateDto } from '../models/aggregations/aggregationCreateDto';
import { ResponseDto } from '../models/responseDto';
import { GetAggregationDto } from '../models/aggregations/getAggregationDto';

@Injectable({
  providedIn: 'root'
})
export class AggregationService {
  private apiUrl = `${environment.apiUrl}/aggregation`;

  constructor(private httpClient: HttpClient) {}

  getAggregations(): Observable<ResponseDto<GetAggregationDto[]>> {
    return this.httpClient.get<ResponseDto<GetAggregationDto[]>>(`${this.apiUrl}/aggregations`);
  }

  getMotherAggregations(levelId: number): Observable<ResponseDto<GetAggregationDto[]>> {
    return this.httpClient.get<ResponseDto<GetAggregationDto[]>>(`${this.apiUrl}/motheraggregations/${levelId}`);
  }

  createAggregation(aggregation: AggregationCreateDto): Observable<ResponseDto<number>> {
    return this.httpClient.post<ResponseDto<number>>(`${this.apiUrl}/aggregation`, aggregation);
  }
}
