import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { PlayersComponent } from './players/players.component';
import { CreatePlayerComponent } from './players/create-player/create-player.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { FactsComponent } from './facts/facts.component';

@NgModule({
  declarations: [
    AppComponent,
    PlayersComponent,
    CreatePlayerComponent,
    FactsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    HttpClientModule,
    FormsModule  
  ],
  providers: [
  
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
