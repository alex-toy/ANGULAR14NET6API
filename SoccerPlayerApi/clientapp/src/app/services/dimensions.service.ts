import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { GetDimensionValuesResultDto } from '../models/dimensionValues/getDimensionValuesResultDto';

@Injectable({
  providedIn: 'root'
})
export class DimensionsService {
  private url = `${environment.apiUrl}/dimension`;

  constructor(private httpClient: HttpClient) {}

  getDimensions(): Observable<GetDimensionsResultDto> {
    return this.httpClient.get<GetDimensionsResultDto>(`${this.url}/dimensions`)
  }

  getDimensionValues(levelId : number): Observable<GetDimensionValuesResultDto> {
    return this.httpClient.get<GetDimensionValuesResultDto>(`${this.url}/dimensionvalues/${levelId}`)
  }
}
