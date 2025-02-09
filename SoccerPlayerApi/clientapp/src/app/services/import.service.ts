import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImportService {

  private url = `${environment.apiUrl}/level`;

  constructor(private httpClient: HttpClient) {}

  createLevel(newLevel: CreateLevelDto): Observable<ResponseDto<number>> {
    return this.httpClient.post<ResponseDto<number>>(`${this.url}/level`, newLevel);
  }
}
