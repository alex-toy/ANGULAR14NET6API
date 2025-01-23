import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { EnvironmentCreateDto } from 'src/app/models/environments/environmentCreateDto';
import { GetDimensionLevelDto } from 'src/app/models/levels/getDimensionLevelDto';
import { EnvironmentService } from 'src/app/services/environment.service';
import { LevelService } from 'src/app/services/level.service';

@Component({
  selector: 'app-create-environment',
  templateUrl: './create-environment.component.html',
  styleUrls: ['./create-environment.component.css']
})
export class CreateEnvironmentComponent {
  dimensionLevels: GetDimensionLevelDto[] = [];
  selectedLevels: { [dimensionId:number] : number} = {};
  environment : EnvironmentCreateDto = {
    name: '',
    description: '',
    levelIdFilter1: null,
    levelIdFilter2: null,
    levelIdFilter3: null,
    levelIdFilter4: null,
    levelIdFilter5: null
  };

  constructor(
    private environmentService: EnvironmentService,
    private levelService: LevelService, 
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchDimensionLevels();
  }

  fetchDimensionLevels(): void {
    this.levelService.getDimensionLevels().subscribe({
      next: result => {
        this.dimensionLevels = result.data;
        this.selectedLevels = this.dimensionLevels.reduce( (acc, curr) => { return { 
          ...acc, [curr.dimensionId] : curr.levels[0].id }
        }, {})
      },
      error: (err) => {
        console.error('Error fetching dimension levels', err);
      }
    });
  }

  onLevelChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedLevelId = +selectElement.value;
    this.selectedLevels[dimensionId] = selectedLevelId;
  }

  onSubmit() {
    let levelFilterIds = Object.values(this.selectedLevels);
    if (levelFilterIds.length >= 1) this.environment.levelIdFilter1 = levelFilterIds[0];
    if (levelFilterIds.length >= 2) this.environment.levelIdFilter2 = levelFilterIds[1];
    if (levelFilterIds.length >= 3) this.environment.levelIdFilter3 = levelFilterIds[2];
    if (levelFilterIds.length >= 4) this.environment.levelIdFilter4 = levelFilterIds[3];
    if (levelFilterIds.length >= 5) this.environment.levelIdFilter5 = levelFilterIds[4];
    
    this.environmentService.createEnvironment(this.environment).subscribe({
      next: (response) => {
        if (response.isSuccess){
          this.router.navigate(['/environments']);
        } else {
          console.log(response.message)
        }
      },
      error: (err) => {
        console.error('Error creating environment:', err);
      }
    });
  }
}
