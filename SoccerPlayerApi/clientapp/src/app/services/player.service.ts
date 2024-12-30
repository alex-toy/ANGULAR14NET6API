import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Player } from '../models/player';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private url = `${environment.apiUrl}/player`;

  constructor(private httpClient : HttpClient) { }

  getById() : Observable<Player> {
    return this.httpClient.get<Player>(`${this.url}/player`)
  }

  getAll() : Observable<Player[]> {
    return this.httpClient.get<Player[]>(`${this.url}/players`)
  }

  create(player : Player) : Observable<number> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post<number>(`${this.url}/player`, player.toJson(), { headers : header })
  }

  update(player : Player) : Observable<number> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.put<number>(`${this.url}/player`, player.toJson(), { headers : header })
  }

  delete(id : number) : Observable<boolean> {
    return this.httpClient.delete<boolean>(`${this.url}/player/${id}`)
  }
}
