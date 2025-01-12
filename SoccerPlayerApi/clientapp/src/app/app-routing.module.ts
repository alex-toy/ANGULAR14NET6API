import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoryComponent } from './history/history.component';
import { ImportComponent } from './import/import.component';
import { SimulationComponent } from './simulation/simulation.component';
import { ExportComponent } from './export/export.component';

const routes: Routes = [
  { path: 'import', component: ImportComponent },
  { path: 'history', component: HistoryComponent },
  { path: 'simulation', component: SimulationComponent },
  { path: 'export', component: ExportComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
