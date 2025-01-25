import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ResponseDto } from '../models/responseDto';
import { SettingsDto } from '../models/settings/settingsDto';
import { SettingCreateDto } from '../models/settings/settingCreateDto';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  private apiUrl = `${environment.apiUrl}/settings`;

  constructor(private http: HttpClient) { }

  getSettings(): Observable<ResponseDto<SettingsDto[]>> {
    return this.http.get<ResponseDto<SettingsDto[]>>(`${this.apiUrl}/settings`);
  }

  updateSetting(setting: SettingsDto): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/update`, setting);
  }

  addSetting(newSetting: SettingCreateDto): Observable<ResponseDto<number>> {
    return this.http.post<ResponseDto<number>>(`${this.apiUrl}/create`, newSetting);
  }
}
