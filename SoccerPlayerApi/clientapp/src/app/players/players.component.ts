import { Component } from '@angular/core';
import { Player } from '../models/player';
import { FormControl, FormGroup } from '@angular/forms';
import { PlayerService } from '../services/player.service';

@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.css']
})
export class PlayersComponent {

  players : Player[] = [new Player(1, "ronaldo", "123"), new Player(2, "mbappe", "556")];
  playerForm = new FormGroup({
    name : new FormControl(''),
    jerseyNumber : new FormControl(null),
  });

  constructor(private playerService : PlayerService) { }

  ngOnInit(){
    this.getPlayers();
  }

  async getPlayers(){
    await this.playerService.getAll().subscribe(data => this.players = data);
  }

  async addPlayer(player : Player){
    await this.playerService.create(player).subscribe(data => console.log(data));
  }

  async deletePlayer(playerId : number){
    await this.playerService.delete(playerId).subscribe(data => console.log(data));
  }
}
