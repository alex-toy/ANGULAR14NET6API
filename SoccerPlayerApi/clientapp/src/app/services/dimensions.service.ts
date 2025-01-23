import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetAggregationsResultDto } from '../models/aggregations/getAggregationsResultDto';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';

@Injectable({
  providedIn: 'root'
})
export class DimensionsService {
  private url = `${environment.apiUrl}/dimension`;

  constructor(private httpClient: HttpClient) {}

  createDimension(newDimension: DimensionDto): Observable<ResponseDto<number>> {
    return this.httpClient.post<ResponseDto<number>>(`${this.url}/create`, newDimension);
  }

  getDimensions(): Observable<ResponseDto<DimensionDto[]>> {
    return this.httpClient.get<ResponseDto<DimensionDto[]>>(`${this.url}/dimensions`)
  }

  getDimensionValues(levelId : number): Observable<GetAggregationsResultDto> {
    return this.httpClient.get<GetAggregationsResultDto>(`${this.url}/dimensionvalues/${levelId}`)
  }

  getDimensionCount(): Observable<ResponseDto<number>> {
    return this.httpClient.get<ResponseDto<number>>(`${this.url}/dimensioncount`)
  }
}
