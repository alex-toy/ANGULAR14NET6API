import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Player } from 'src/app/models/player';

@Component({
  selector: 'app-create-player',
  templateUrl: './create-player.component.html',
  styleUrls: ['./create-player.component.css']
})
export class CreatePlayerComponent {

  player : Player | undefined = Input();
  playerForm : FormGroup;

  constructor() {
    this.playerForm = new FormGroup({
      name : new FormControl(this.player?.name ?? ''),
      jerseyNumber : new FormControl(this.player?.jerseyNumber ?? ''),
    });
  }

  ngOnInit(){
  }

  addPlayer(){

  }
}
