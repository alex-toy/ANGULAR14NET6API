import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ImportFactDto } from '../models/imports/ImportFactDto';
import { Observable } from 'rxjs';
import { ResponseDto } from '../models/responseDto';
import { ImportFactCreateResultDto } from '../models/imports/ImportFactCreateResultDto';

@Injectable({
  providedIn: 'root'
})
export class ImportService {

  private url = `${environment.apiUrl}/import`;

  constructor(private httpClient: HttpClient) {}

  createImportFact(newLevel: ImportFactDto[]): Observable<ResponseDto<ImportFactCreateResultDto>> {
    return this.httpClient.post<ResponseDto<ImportFactCreateResultDto>>(`${this.url}/createimportfact`, newLevel);
  }
}
