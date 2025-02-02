import { Component } from '@angular/core';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { ResponseDto } from '../models/responseDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { AggregationService } from '../services/aggregation.service';
import { GetAggregationDto } from '../models/aggregations/getAggregationDto';

@Component({
  selector: 'app-aggregation',
  templateUrl: './aggregation.component.html',
  styleUrls: ['./aggregation.component.css']
})
export class AggregationComponent {
  dimensions: DimensionDto[] = [];
  levels: GetLevelDto[] = [];
  motherAggregations: GetAggregationDto[] = [];
  selectedMotherAggregationId : number = 0;
  isLoading: boolean = true;

  constructor(
    private dimensionService: DimensionsService,
    private aggregationService: AggregationService,
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

  fetchMotherAggregations(levelId: number): void {
    this.aggregationService.getMotherAggregations(levelId).subscribe({
      next: (response: ResponseDto<GetAggregationDto[]>) => {
        this.motherAggregations = response.data;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  onDimensionChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const dimensionId = +selectElement.value;
    if (dimensionId === 0) {
      this.levels = [];
      this.motherAggregations = [];
      this.selectedMotherAggregationId = 0;
      return;
    }
    const selectedDimension = this.dimensions.filter(x => x.id === dimensionId)[0];
    this.levels = selectedDimension.levels;
  }

  onLevelChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const levelId = +selectElement.value;
    if (levelId === 0) {
      this.motherAggregations = [];
      this.selectedMotherAggregationId = 0;
      return;
    }
    this.fetchMotherAggregations(+selectElement.value)
  }

  onMotherAggregationsChange(event: Event) {
    this.selectedMotherAggregationId = +(event.target as HTMLSelectElement).value;
    console.log("Selected client ID:", this.selectedMotherAggregationId);
  }
}
