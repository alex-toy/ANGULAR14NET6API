import { Component } from '@angular/core';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { ResponseDto } from '../models/responseDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { AggregationService } from '../services/aggregation.service';
import { GetAggregationDto } from '../models/aggregations/getAggregationDto';
import { AggregationCreateDto } from '../models/aggregations/aggregationCreateDto';

@Component({
  selector: 'app-aggregation',
  templateUrl: './aggregation.component.html',
  styleUrls: ['./aggregation.component.css']
})
export class AggregationComponent {
  dimensions: DimensionDto[] = [];
  levels: GetLevelDto[] = [];
  selectedLevelId: number = 0;
  selectedLevel: number = 0;
  motherAggregations: GetAggregationDto[] = [];
  selectedMotherAggregationId : number = 0;
  newAggregationValue: string = '';
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
  
  createAggregation(): void {
    const aggregationDto: AggregationCreateDto = {
      levelId: this.selectedLevelId,
      motherAggregationId: this.selectedMotherAggregationId,
      value: this.newAggregationValue
    };
    this.aggregationService.createAggregation(aggregationDto).subscribe({
      next: (response) => {
        console.log(response)
      },
      error: (err) => {
        console.error('Error adding new setting:', err);
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
    this.levels = selectedDimension.levels.filter(l => l.ancestorId !== null);
  }

  onLevelChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedLevelId = +selectElement.value;
    if (this.selectedLevelId === 0) {
      this.motherAggregations = [];
      this.selectedMotherAggregationId = 0;
      return;
    }
    this.fetchMotherAggregations(+selectElement.value)
  }

  onMotherAggregationsChange(event: Event) {
    this.selectedMotherAggregationId = +(event.target as HTMLSelectElement).value;
  }

  isFormValid(): boolean {
    return this.selectedLevelId > 0 && this.selectedMotherAggregationId > 0 && this.newAggregationValue.trim().length > 0;
  }

  // Method to handle form submission
  onSubmit(): void {
    if (!this.isFormValid()) {
      console.log('Form is not valid');
      return;
    }
      
    this.createAggregation();
    this.selectedLevel = 0;
    this.motherAggregations = [];
    this.newAggregationValue = '';
  }
}
