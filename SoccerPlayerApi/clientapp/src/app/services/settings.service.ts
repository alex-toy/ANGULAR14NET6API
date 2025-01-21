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

  getHistoryDatesMonth(presentDate : string, pastSpan : number): string[] {

    let year : number = +presentDate.substring(0, 4);
    let month : number = +presentDate.substring(5, 7);

    let dates: string[] = [presentDate.substring(0,7)];

    while(pastSpan > 1) {
      month = (month - 1)%12;
      if (month == 0) {
        month = 12;
        year = year - 1;
      }
        
      dates.unshift(`${year}-${(month+"").length == 1 ? "0"+month : month}`);
      pastSpan -= 1;
    }

    return dates;
  }

  getHistoryDatesWeek(presentDate : string, pastSpan : number): string[] {

    let year : number = +presentDate.substring(0, 4);
    let week : number = +presentDate.substring(6, 8);

    let dates: string[] = [presentDate.substring(0,8)];

    while(pastSpan > 1) {
      week = (week - 1)%52;
      if (week == 0) {
        week = 52;
        year = year - 1;
      }
      
      dates.unshift(`${year}-S${(week+"").length == 1 ? "0"+week : week}`);
      pastSpan -= 1;
    }

    return dates;
  }
}
