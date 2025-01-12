import { Component } from '@angular/core';
import { ScopeDto } from '../models/scopes/scopeDto';
import { HistoryService } from '../services/history.service';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { GetDimensionValueDto } from '../models/dimensionValues/getDimensionValueDto';
import { FactService } from '../services/fact.service';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { GetDimensionValuesResultDto } from '../models/dimensionValues/getDimensionValuesResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  scopes: ScopeDto[] = [];
  isLoading: boolean = true;
  
  dimensions: DimensionDto[] = [];
  dimensionLevels: GetDimensionLevelDto[] = [];

  selectedLevels: { [dimensionId:number] : number} = {};
  selectedDimensionValues: { [dimensionId:number] : number} = {};
  
  constructor(
    private historyService: HistoryService,
    private levelService: LevelService, 
    private dimensionService: DimensionsService, 
  ) {}

  ngOnInit(): void {
    this.fetchScopes();
    this.fetchDimensions();
    this.fetchDimensionLevels();
  }
  
  fetchScopes(): void {
    this.historyService.getScopes().subscribe({
      next: (response: ResponseDto<ScopeDto[]>) => {
        this.scopes = response.data.map(d => new ScopeDto(d));
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  onLevelChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedLevelId = +selectElement.value;
    this.selectedLevels[dimensionId] = selectedLevelId;
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
}
