import { Component } from '@angular/core';
import { EnvironmentService } from '../services/environment.service';
import { ResponseDto } from '../models/responseDto';
import { EnvironmentDto } from '../models/environments/environmentDto';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';

@Component({
  selector: 'app-environments',
  templateUrl: './environments.component.html',
  styleUrls: ['./environments.component.css']
})
export class EnvironmentsComponent {
  environments: EnvironmentDto[] = [];
  dimensions: DimensionDto[] = [];
  isLoading: boolean = true;
  errorMessage: string = '';

  constructor(
    private environmentService: EnvironmentService,
    private dimensionService: DimensionsService,
  ) {}

  ngOnInit(): void {
    this.fetchDimensions();
    this.fetchEnvironments();
  }

  fetchDimensions(): void {
    this.dimensionService.getDimensions().subscribe({
      next: (response: ResponseDto<DimensionDto[]>) => {
        this.dimensions = response.data;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchEnvironments(): void {
    this.isLoading = true;
    this.environmentService.getEnvironments().subscribe({
      next: (response: ResponseDto<EnvironmentDto[]>) => {
        this.environments = response.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Error fetching environments. Please try again.';
        this.isLoading = false;
      }
    });
  }

  deleteEnvironment(id: number): void {
    if (confirm('Are you sure you want to delete this environment?')) {
      this.environmentService.deleteEnvironment(id).subscribe({
        next: () => {
          // Filter out the deleted environment from the list
          this.environments = this.environments.filter(env => env.id !== id);
        },
        error: (err) => {
          console.error('Error deleting environment', err);
          this.errorMessage = 'Error deleting environment. Please try again.';
        }
      });
    }
  }
}
