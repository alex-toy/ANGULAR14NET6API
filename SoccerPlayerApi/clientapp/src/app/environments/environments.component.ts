import { Component } from '@angular/core';
import { EnvironmentService } from '../services/environment.service';
import { ResponseDto } from '../models/responseDto';
import { EnvironmentDto as FrameDto } from '../models/environments/environmentDto';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { EnvironmentUpdateDto } from '../models/environments/environmentUpdateDto';
import { ActivatedRoute, Router } from '@angular/router';
import { AlgorithmDto } from '../models/simulations/algorithms/algorithmDto';
import { FrameSimulationCreateDto } from '../models/simulations/FrameSimulationCreateDto';
import { SimulationService } from '../services/simulation.service';
import { AlgorithmParameterValueDto } from '../models/simulations/algorithms/algorithmParameterValueDto';

@Component({
  selector: 'app-environments',
  templateUrl: './environments.component.html',
  styleUrls: ['./environments.component.css']
})
export class EnvironmentsComponent {
  environments: FrameDto[] = [];
  dimensions: DimensionDto[] = [];
  isLoading: boolean = true;
  errorMessage: string = '';
  isSimulationModalOpen: boolean = false;
  algorithms : AlgorithmDto[] = [];
  selectedAlgorithm: AlgorithmDto | null = null;
  userInputValueDto : AlgorithmParameterValueDto[] = [];

  simulationCreateDto: FrameSimulationCreateDto = new FrameSimulationCreateDto(0, 0, []);

  constructor(
    private environmentService: EnvironmentService,
    private dimensionService: DimensionsService,
    private simulationService: SimulationService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchDimensions();
    this.fetchEnvironments();
    this.fetchAlgorithms();
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

  fetchAlgorithms(): void {
    this.simulationService.getAlgorithms().subscribe({
      next: (response: ResponseDto<AlgorithmDto[]>) => {
        this.algorithms = response.data;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchEnvironments(): void {
    this.isLoading = true;
    this.environmentService.getEnvironments().subscribe({
      next: (response: ResponseDto<FrameDto[]>) => {
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
          this.environments = this.environments.filter(env => env.id !== id);
        },
        error: (err) => {
          console.error('Error deleting environment', err);
          this.errorMessage = 'Error deleting environment. Please try again.';
        }
      });
    }
  }

  updateEnvironment(environementId: number): void {
    let environment: FrameDto = this.environments.find(e => e.id === environementId)!;
    let updateEnvironment = { ...environment } as EnvironmentUpdateDto;

    this.environmentService.updateEnvironment(updateEnvironment).subscribe({
      next: () => {
        this.router.navigate(['/environments']);
      },
      error: (err) => {
        console.error('Error updating environment', err);
        this.errorMessage = 'Error updating environment. Please try again.';
      }
    });
  }

  openSimulationModal(frameId: number): void {
    this.simulationCreateDto.frameId = frameId;
    this.isSimulationModalOpen = true;
  }

  closeSimulationModal(): void {
    this.simulationCreateDto.frameId = 0;
    this.isSimulationModalOpen = false;
  }

  createSimulation(): void {
    const simulationData = new FrameSimulationCreateDto(
      this.simulationCreateDto.frameId,
      this.simulationCreateDto.algorithmId,
      this.simulationCreateDto.values
    );

    // call service
  }
}
