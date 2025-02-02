import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FactsComponent } from './facts/facts.component';
import { MatDialogModule } from '@angular/material/dialog';
import { CreateFactComponent } from './facts/create-fact/create-fact.component';
import { ScopesComponent } from './scopes/scopes.component';
import { HistoryComponent } from './history/history.component';
import { ImportComponent } from './import/import.component';
import { SimulationComponent } from './simulation/simulation.component';
import { ExportComponent } from './export/export.component';
import { FilterByTimeLabelPipe } from './pipes/filter-by-time-label.pipe';
import { EnvironmentsComponent } from './environments/environments.component';
import { EnvironmentDetailsComponent } from './environments/environment-details/environment-details.component';
import { CreateEnvironmentComponent } from './environments/create-environment/create-environment.component';
import { SettingsComponent } from './settings/settings.component';
import { LevelModalComponent } from './settings/add-level-modal/add-level-modal.component';
import { AggregationComponent } from './aggregation/aggregation.component';


@NgModule({
  declarations: [
    AppComponent,
    FactsComponent,
    CreateFactComponent,
    ScopesComponent,
    HistoryComponent,
    ImportComponent,
    SimulationComponent,
    ExportComponent,
    FilterByTimeLabelPipe,
    EnvironmentsComponent,
    EnvironmentDetailsComponent,
    CreateEnvironmentComponent,
    SettingsComponent,
    LevelModalComponent,
    AggregationComponent 
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatDialogModule,
    ReactiveFormsModule
  ],
  providers: [
  
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
