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
  newPlayer : Player = new Player(0, "", "");

  constructor(private playerService : PlayerService) { }

  async ngOnInit(){
    await this.getPlayers();
  }

  async getPlayers(){
    await this.playerService.getAll().subscribe(data => this.players = data);
  }

  async addPlayer(player : Player){
    await this.playerService.create(player).subscribe(data => console.log(data));
  }

  async deletePlayer(playerId : number){
    await this.playerService.delete(playerId).subscribe(data => this.players = this.players.filter(player => player.id !== playerId));
  }

  async updatePlayer(player: Player) {
    const updatedPlayer = new Player(player.id, this.newPlayer.name, this.newPlayer.jerseyNumber);
    await this.playerService.update(updatedPlayer).subscribe(data => {
      this.newPlayer = new Player(0, "", "");
      const index = this.players.findIndex(p => p.id === player.id);
      if (index !== -1) this.players[index] = updatedPlayer;
    });
  }

  async createPlayer() {
    if (this.newPlayer.name && this.newPlayer.jerseyNumber) {
      await this.playerService.create(this.newPlayer).subscribe(data => {
        let temp = new Player(data, this.newPlayer.name, this.newPlayer.jerseyNumber);
        this.players.push(new Player(data, this.newPlayer.name, this.newPlayer.jerseyNumber));
        this.newPlayer = new Player(0, "", "");
      });
    }
  }
}
