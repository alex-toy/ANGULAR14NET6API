import { Component } from '@angular/core';
import { FactService } from '../services/fact.service';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { GetDimensionValuesResultDto } from '../models/dimensionValues/getDimensionValuesResultDto';
import { GetDimensionValueDto } from '../models/dimensionValues/getDimensionValueDto';
import { GetFactTypesResultDto } from '../models/facts/getFactTypesResultDto';
import { MatDialog } from '@angular/material/dialog';
import { CreateFactComponent } from './create-fact/create-fact.component';

@Component({
  selector: 'app-facts',
  templateUrl: './facts.component.html',
  styleUrls: ['./facts.component.css']
})
export class FactsComponent {
  factsResult: GetFactsResultDto | null = null;
  filter: GetFactFilterDto = {
    type: '',
    factDimensionFilters: [],
    dimensionValueIds: []
  };
  
  factTypes : string[] = [];
  dimensions: DimensionDto[] = [];
  dimensionLevels: GetDimensionLevelDto[] = [];
  dimensionValues: { [dimensionId:number] : GetDimensionValueDto[]} = {};

  selectedLevels: { [dimensionId:number] : number} = {};
  selectedDimensionValues: { [dimensionId:number] : number} = {};

  constructor(
    private factService: FactService, 
    private levelService: LevelService, 
    private dimensionService: DimensionsService, 
    public dialog: MatDialog) {}

  ngOnInit(): void {
    this.fetchFactTypes();
    this.fetchDimensions();
    this.fetchDimensionLevels();
    this.fetchFacts();
  }

  openCreateFactModal(): void {
    const dialogRef = this.dialog.open(CreateFactComponent, {
      width: '800px',
    });

    // Optionally, handle actions when the modal is closed
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  fetchFacts(): void {
    this.factService.getFacts(this.filter).subscribe({
      next: (data: GetFactsResultDto) => {
        this.factsResult = data;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  fetchFactTypes(): void {
    this.factService.getFactTypes().subscribe({
      next: (data: GetFactTypesResultDto) => {
        this.factTypes = data.types;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  fetchDimensions(): void {
    this.dimensionService.getDimensions().subscribe({
      next: (response: GetDimensionsResultDto) => {
        this.dimensions = response.dimensions;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchDimensionValues(levelId: number, dimensionId: number): void {
    this.dimensionService.getDimensionValues(levelId).subscribe({
      next: (response: GetDimensionValuesResultDto) => {
        this.dimensionValues[dimensionId] = response.dimensionValues;
      },
      error: (err) => {
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
      },
      error: (err) => {
        console.error('Error fetching dimension levels', err);
      }
    });
  }
  
  applyFilter(): void {
    this.fetchFacts();
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
    this.filter.dimensionValueIds = Object.values(this.selectedDimensionValues);
  }

  onfactTypeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.filter.type = selectElement.value;
  }
}
