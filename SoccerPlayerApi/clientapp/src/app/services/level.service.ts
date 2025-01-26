import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ResponseDto } from '../models/responseDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { CreateLevelDto } from '../models/levels/createLevelDto';

@Injectable({
  providedIn: 'root'
})
export class LevelService {

  private url = `${environment.apiUrl}/level`;

  constructor(private httpClient: HttpClient) {}

  createLevel(newLevel: CreateLevelDto): Observable<ResponseDto<number>> {
    return this.httpClient.post<ResponseDto<number>>(`${this.url}/level`, newLevel);
  }

  getLevels(dimensionId: number): Observable<ResponseDto<GetLevelDto[]>> {
    return this.httpClient.get<ResponseDto<GetLevelDto[]>>(`${this.url}/levels/${dimensionId}`);
  }

  getTimeLevels(): Observable<ResponseDto<GetLevelDto[]>> {
    return this.httpClient.get<ResponseDto<GetLevelDto[]>>(`${this.url}/timelevels`);
  }
  
  getDimensionLevels(): Observable<ResponseDto<GetDimensionLevelDto[]>> {
    return this.httpClient.get<ResponseDto<GetDimensionLevelDto[]>>(`${this.url}/dimensionlevels`);
  }
}
