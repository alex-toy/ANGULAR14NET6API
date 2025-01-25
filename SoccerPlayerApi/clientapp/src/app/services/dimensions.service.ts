import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetAggregationsResultDto } from '../models/aggregations/getAggregationsResultDto';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { AggregationCreateDto } from '../models/aggregations/aggregationCreateDto';

@Injectable({
  providedIn: 'root'
})
export class DimensionsService {
  private apiUrl = `${environment.apiUrl}/dimension`;

  constructor(private httpClient: HttpClient) {}

  createDimension(newDimension: DimensionDto): Observable<ResponseDto<number>> {
    return this.httpClient.post<ResponseDto<number>>(`${this.apiUrl}/create`, newDimension);
  }

  getDimensions(): Observable<ResponseDto<DimensionDto[]>> {
    return this.httpClient.get<ResponseDto<DimensionDto[]>>(`${this.apiUrl}/dimensions`)
  }

  getDimensionValues(levelId : number): Observable<GetAggregationsResultDto> {
    return this.httpClient.get<GetAggregationsResultDto>(`${this.apiUrl}/dimensionvalues/${levelId}`)
  }

  getDimensionCount(): Observable<ResponseDto<number>> {
    return this.httpClient.get<ResponseDto<number>>(`${this.apiUrl}/dimensioncount`)
  }

  getAggregations(): Observable<{ data: AggregationCreateDto[] }> {
    return this.httpClient.get<{ data: AggregationCreateDto[] }>(`${this.apiUrl}/aggregations`);
  }

  createAggregation(aggregation: AggregationCreateDto): Observable<{ data: AggregationCreateDto }> {
    return this.httpClient.post<{ data: AggregationCreateDto }>(`${this.apiUrl}/aggregation`, aggregation);
  }
}
