import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoryComponent } from './history/history.component';
import { ImportComponent } from './import/import.component';
import { SimulationComponent } from './simulation/simulation.component';
import { ExportComponent } from './export/export.component';
import { EnvironmentsComponent } from './environments/environments.component';
import { EnvironmentDetailsComponent } from './environments/environment-details/environment-details.component';
import { CreateEnvironmentComponent } from './environments/create-environment/create-environment.component';
import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
  { path: 'environments', component: EnvironmentsComponent },
  { path: 'environment-details/:id', component: EnvironmentDetailsComponent },
  { path: 'create-environment', component: CreateEnvironmentComponent },

  { path: 'import', component: ImportComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'simulation', component: SimulationComponent },
  { path: 'export', component: ExportComponent },
  { path: 'settings', component: SettingsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
