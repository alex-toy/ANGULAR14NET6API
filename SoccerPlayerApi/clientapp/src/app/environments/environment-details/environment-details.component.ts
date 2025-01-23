import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EnvironmentService } from 'src/app/services/environment.service';
import { EnvironmentDto } from 'src/app/models/environments/environmentDto';
import { EnvironmentUpdateDto } from 'src/app/models/environments/environmentUpdateDto';
import { LevelService } from 'src/app/services/level.service';
import { GetDimensionLevelDto } from 'src/app/models/levels/getDimensionLevelDto';

@Component({
  selector: 'app-environment-details',
  templateUrl: './environment-details.component.html',
  styleUrls: ['./environment-details.component.css']
})
export class EnvironmentDetailsComponent {
  environment: EnvironmentDto | undefined;
  dimensionLevels: GetDimensionLevelDto[] = [];
  selectedLevels: { [dimensionId:number] : number} = {};
  isLoading: boolean = true;
  isEditMode: boolean = false;  // Flag to toggle between view and edit mode
  environmentForm: FormGroup;

  constructor(
    private environmentService: EnvironmentService,
        private levelService: LevelService, 
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    // Initialize form
    this.environmentForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      levelIdFilter1: [0, Validators.required],
      levelIdFilter2: [null],
      levelIdFilter3: [null],
      levelIdFilter4: [null],
      levelIdFilter5: [null]
    });
  }

  ngOnInit(): void {
    this.fetchEnvironmentDetails();
    this.fetchDimensionLevels();
  }

  fetchEnvironmentDetails(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));  // Retrieve ID from route
    this.environmentService.getEnvironmentById(id).subscribe({
      next: (response) => {
        this.environment = response.data;
        this.isLoading = false;
        this.initializeForm();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
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

  // Initialize form with environment details
  initializeForm(): void {
    if (this.environment) {
      this.environmentForm.patchValue({
        name: this.environment.name,
        description: this.environment.description || '',
        levelIdFilter1: this.environment.levelIdFilter1,
        levelIdFilter2: this.environment.levelIdFilter2,
        levelIdFilter3: this.environment.levelIdFilter3,
        levelIdFilter4: this.environment.levelIdFilter4,
        levelIdFilter5: this.environment.levelIdFilter5
      });
    }
  }

  toggleEditMode(): void {
    this.isEditMode = !this.isEditMode;
  }

  onLevelChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedLevelId = +selectElement.value;
    this.selectedLevels[dimensionId] = selectedLevelId;
  }

  // Update the environment with the form values
  updateEnvironment(): void {
    if (this.environmentForm.valid && this.environment) {
      const updatedEnvironment: EnvironmentUpdateDto = {
        id: this.environment.id,
        name: this.environmentForm.value.name,
        description: this.environmentForm.value.description,
        levelIdFilter1: this.environmentForm.value.levelIdFilter1,
        levelIdFilter2: this.environmentForm.value.levelIdFilter2,
        levelIdFilter3: this.environmentForm.value.levelIdFilter3,
        levelIdFilter4: this.environmentForm.value.levelIdFilter4,
        levelIdFilter5: this.environmentForm.value.levelIdFilter5
      };

      this.environmentService.updateEnvironment(updatedEnvironment).subscribe({
        next: () => {
          this.isEditMode = false;  // Turn off edit mode after successful update
          this.fetchEnvironmentDetails();  // Refresh environment data
        },
        error: (err) => {
          console.error('Error updating environment:', err);
        }
      });
    }
  }
}
