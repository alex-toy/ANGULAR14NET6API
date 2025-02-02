import { Component } from '@angular/core';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { ResponseDto } from '../models/responseDto';
import { LevelService } from '../services/level.service';
import { GetLevelDto } from '../models/levels/getLevelDto';

@Component({
  selector: 'app-aggregation',
  templateUrl: './aggregation.component.html',
  styleUrls: ['./aggregation.component.css']
})
export class AggregationComponent {
  dimensions: DimensionDto[] = [];
  levels: GetLevelDto[] = [];
  isLoading: boolean = true;

  constructor(
    private dimensionService: DimensionsService,
    private levelService: LevelService,
  ) {}

  ngOnInit(): void {
    this.fetchDimensions();
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

  fetchLevels(dimensionId : number): void {
    this.levelService.getLevels(dimensionId).subscribe({
      next: (response) => {
        this.levels = response.data
      },
      error: (err) => {
        console.error('Error fetching settings:', err);
      }
    });
  }

  onDimensionChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.fetchLevels(+selectElement.value);
  }

  onLevelChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    console.log(+selectElement.value)
  }
}
