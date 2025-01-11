import { Component } from '@angular/core';
import { FactService } from '../services/fact.service';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { LevelService } from '../services/level.service';
import { GetLevelsResultDto } from '../models/levels/getLevelsResultDto';
import { DimensionsService } from '../services/dimensions.service';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { GetDimensionValuesResultDto } from '../models/dimensionValues/getDimensionValuesResultDto';
import { GetDimensionValueDto } from '../models/dimensionValues/getDimensionValueDto';

@Component({
  selector: 'app-facts',
  templateUrl: './facts.component.html',
  styleUrls: ['./facts.component.css']
})
export class FactsComponent {
  factsResult: GetFactsResultDto | null = null;
  errorMessage: string = '';
  filter: GetFactFilterDto = {
    type: '',
    factDimensionFilters: [],
    dimensionValueIds: []
  };
  levels: GetLevelDto[] = [];
  dimensionLevels: GetDimensionLevelDto[] = [];
  dimensions: DimensionDto[] = [];
  selectedLevels: { [dimensionId:number] : number} = {};
  fetchedDimensionValues: { [dimensionId:number] : GetDimensionValueDto[]} = {};
  selectedDimensionValues: { [dimensionId:number] : number} = {};

  constructor(private factService: FactService, private levelService: LevelService, private dimensionService: DimensionsService) {}

  ngOnInit(): void {
    this.fetchDimensionLevels();
    this.fetchDimensions();
    this.fetchLevels();
    this.fetchFacts();
  }

  fetchFacts(): void {
    this.factService.getFacts(this.filter).subscribe({
      next: (data: GetFactsResultDto) => {
        this.factsResult = data;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load facts';
        console.error(err);
      }
    });
  }

  applyFilter(): void {
    this.filter.type = 'sales';
    this.filter.dimensionValueIds = Object.values(this.selectedDimensionValues);
    this.fetchFacts();
  }

  fetchLevels(): void {
    const dimensionId = 1;
    this.levelService.getLevels(dimensionId).subscribe({
      next: (response: GetLevelsResultDto) => {
        this.levels = response.levels;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load levels. Please try again later.';
        console.error('Error fetching levels', err);
      }
    });
  }

  onLevelChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedLevelId = +selectElement.value;
    this.selectedLevels[dimensionId] = selectedLevelId;
    this.fetchDimensionValues(selectedLevelId, dimensionId)
  }

  onDimensionValueChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedDimensionValueId = +selectElement.value;
    this.selectedDimensionValues[dimensionId] = selectedDimensionValueId;
  }

  fetchDimensions(): void {
    this.dimensionService.getDimensions().subscribe({
      next: (response: GetDimensionsResultDto) => {
        this.dimensions = response.dimensions;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load dimensions. Please try again later.';
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchDimensionValues(levelId: number, dimensionId: number): void {
    this.dimensionService.getDimensionValues(levelId).subscribe({
      next: (response: GetDimensionValuesResultDto) => {
        this.fetchedDimensionValues[dimensionId] = response.dimensionValues;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load dimensions. Please try again later.';
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchDimensionLevels(): void {
    this.levelService.getDimensionLevels().subscribe({
      next: dimensionLevelResult => {
        this.dimensionLevels = dimensionLevelResult.dimensionLevels;
        this.selectedLevels = this.dimensionLevels.reduce( (acc, curr) => { return { 
          ...acc, [curr.dimensionId] : curr.levels[0].id }
        }, {})
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = 'Failed to load dimension levels'; // Handle errors
        console.error('Error fetching dimension levels', err);
      }
    });
  }
}
