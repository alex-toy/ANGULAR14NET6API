import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Player } from '../models/player';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private url = environment.apiUrl

  constructor(private httpClient : HttpClient) { }

  getAll() : Observable<Player[]> {
    return this.httpClient.get<Player[]>(`${this.url}/players`)
  }

  create(player : Player) : Observable<any> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.post(`${this.url}/player`, player.toJson(), { headers : header })
  }

  edit(player : Player) : Observable<any> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.put(`${this.url}/player`, player.toJson(), { headers : header })
  }

  delete(id : number) : Observable<any> {
    const header = new HttpHeaders().set('Content-type', 'application/json');
    return this.httpClient.delete(`${this.url}/player/${id}`)
  }
}
