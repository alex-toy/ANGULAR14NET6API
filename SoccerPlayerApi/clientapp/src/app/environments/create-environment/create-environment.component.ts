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
    dimension1Id : 0,
    levelIdFilter1: 0,
    dimension2Id : null,
    levelIdFilter2: null,
    dimension3Id : null,
    levelIdFilter3: null,
    dimension4Id : null,
    levelIdFilter4: null,
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

    let dimensionCounter = 1;
    for (const [dimensionId, levelId] of Object.entries(this.selectedLevels)) {
      if (dimensionCounter == 1) {
        this.environment.dimension1Id = +dimensionId;
        this.environment.levelIdFilter1 = levelId;
      }

      if (dimensionCounter == 2) {
        this.environment.dimension2Id = +dimensionId;
        this.environment.levelIdFilter2 = levelId;
      }

      if (dimensionCounter == 3) {
        this.environment.dimension3Id = +dimensionId;
        this.environment.levelIdFilter3 = levelId;
      }

      if (dimensionCounter == 4) {
        this.environment.dimension4Id = +dimensionId;
        this.environment.levelIdFilter4 = levelId;
      }

      dimensionCounter++;
    }
    
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
