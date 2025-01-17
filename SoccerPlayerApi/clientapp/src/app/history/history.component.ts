import { Component } from '@angular/core';
import { ScopeDto } from '../models/scopes/scopeDto';
import { HistoryService } from '../services/history.service';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { ScopeFilterDto } from '../models/scopes/scopeFilterDto';
import { ScopeDimensionFilterDto } from '../models/scopes/scopeDimensionFilterDto';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  scopes: ScopeDto[] = [];
  scopeData: GetScopeDataDto[] = [];
  isLoading: boolean = true;
  selectedTimeLabel = 'YEAR';
  
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
  
  applyFilter(): void {
    this.fetchScopes();
  }
  
  fetchScopes(): void {
    let filter = {
      scopeDimensionFilters : Object.entries(this.selectedLevels).map(x => new ScopeDimensionFilterDto(+x[0], x[1]))
    } as ScopeFilterDto;
    this.historyService.getScopes(filter).subscribe({
      next: (response: ResponseDto<ScopeDto[]>) => {
        this.scopes = response.data.map(d => new ScopeDto(d));
        console.log(this.scopes)
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

  onSelectScope(scope : ScopeDto){
    this.historyService.getScopeData(scope).subscribe({
      next: (response: ResponseDto<GetScopeDataDto[]>) => {
        this.scopeData = response.data;
        console.log(this.scopeData)
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  get uniqueYears() {
    const years = this.scopeData
      .filter(item => item.timeDimension.timeAggregationLabel === 'YEAR')
      .map(item => item.timeDimension.timeAggregationValue);
    return [...new Set(years)];
  }

  get uniqueTypes() {
    const types = this.scopeData
      .filter(item => item.timeDimension.timeAggregationLabel === 'YEAR')
      .map(item => item.type);
    return [...new Set(types)]; // Remove duplicates
  }

  getAmountForTypeAndYear(type: string, year: string): number | null {
    const item = this.scopeData.find(
      (data) => data.type === type && data.timeDimension.timeAggregationValue === year
    );
    return item ? item.amount : null;
  }
}
