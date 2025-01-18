import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { GetAggregationsResultDto } from '../models/aggregations/getAggregationsResultDto';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';

@Injectable({
  providedIn: 'root'
})
export class DimensionsService {
  private url = `${environment.apiUrl}/dimension`;

  constructor(private httpClient: HttpClient) {}

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
