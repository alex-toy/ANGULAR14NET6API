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
import { MatDialogModule } from '@angular/material/dialog';
import { CreateFactComponent } from './facts/create-fact/create-fact.component';
import { ScopesComponent } from './scopes/scopes.component';
import { HistoryComponent } from './history/history.component';
import { ImportComponent } from './import/import.component';
import { SimulationComponent } from './simulation/simulation.component';
import { ExportComponent } from './export/export.component';
import { FilterByTimeLabelPipe } from './pipes/filter-by-time-label.pipe';
import { EnvironmentsComponent } from './environments/environments.component'; // Your modal component


@NgModule({
  declarations: [
    AppComponent,
    PlayersComponent,
    CreatePlayerComponent,
    FactsComponent,
    CreateFactComponent,
    ScopesComponent,
    HistoryComponent,
    ImportComponent,
    SimulationComponent,
    ExportComponent,
    FilterByTimeLabelPipe,
    EnvironmentsComponent 
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatDialogModule 
  ],
  providers: [
  
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
