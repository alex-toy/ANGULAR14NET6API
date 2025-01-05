import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetLevelsResultDto } from '../models/levels/getLevelsResultDto';

@Injectable({
  providedIn: 'root'
})
export class LevelService {

  private url = `${environment.apiUrl}/level`;

  constructor(private http: HttpClient) {}

  getLevels(dimensionId: number): Observable<GetLevelsResultDto> {
    return this.http.get<GetLevelsResultDto>(`${this.url}/levels/${dimensionId}`);
  }
}
