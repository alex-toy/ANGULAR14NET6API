import { Component } from '@angular/core';
import { ScopeDto } from '../models/scopes/scopeDto';
import { HistoryService } from '../services/history.service';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { ScopeFilterDto } from '../models/scopes/scopeFilterDto';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';
import { EnvironmentDto } from '../models/environments/environmentDto';
import { EnvironmentService } from '../services/environment.service';
import { SettingsService } from '../services/settings.service';
import { SettingsDto } from '../models/settings/settingsDto';
import { FactService } from '../services/fact.service';
import { FactCreateDto } from '../models/facts/factCreateDto';
import { FactUpdateDto } from '../models/facts/factUpdateDto';
import { TimeAggregationDto } from '../models/facts/timeAggregationDto';
import { DataTypeDto } from '../models/facts/DataTypeDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { EnvironmentScopeDto } from '../models/scopes/environmentScopeDto';
import { scopeByDataTypeDto } from '../models/scopes/scopeByDataTypeDto';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  settings: SettingsDto[] = [];
  isEditing: { [key: string]: boolean } = {};

  scopes: EnvironmentScopeDto[] = [];
  selectedScope: EnvironmentScopeDto | null = null;
  scopeDataByDataType: scopeByDataTypeDto = {};
  types: DataTypeDto[] = [];

  filterMode: string = 'dimensions';
  isLoading: boolean = true;

  selectedTimeLabel = 'YEAR';
  timeLevels : GetLevelDto[] = [];
  selectedTimeLevel : GetLevelDto = { id : 0, dimensionId : 0, label : "" };

  selectedTimeAggregationLabel: string = 'YEAR';
  timeAggregationDtos : TimeAggregationDto[] = [];
  
  dimensions: DimensionDto[] = [];
  dimensionLevels: GetDimensionLevelDto[] = [];

  selectedLevels: { [dimensionId:number] : number} = {};
  selectedDimensionValues: { [dimensionId:number] : number} = {};
  
  environments: EnvironmentDto[] = [];
  selectedEnvironmentId : number = 0;
  
  constructor(
    private historyService: HistoryService,
    private levelService: LevelService, 
    private dimensionService: DimensionsService,
    private environmentService: EnvironmentService,
    private settingsService: SettingsService,
    private factService: FactService,
  ) {}

  ngOnInit(): void {
    this.fetchTimeLevels();
    this.fetchSettings();
    this.fetchDimensions();
    this.fetchDimensionLevels();
    this.fetchEnvironments();
    this.fetchDataTypes();
  }
  
  applyLevelFilter(): void {
    this.fetchScopes();
  }

  applyEnvironmentFilter() {
    this.fetchScopesByEnvironment();
  }
  
  fetchScopes(): void {
    const selectedLevels = Object.keys(this.selectedLevels).map(key => ({ dimId: key, levelId: this.selectedLevels[+key] }));
    let filter = {
      Dimension1Id: selectedLevels[0] ? +selectedLevels[0].dimId : null,
      Level1Id: selectedLevels[0] ? +selectedLevels[0].levelId : null,
    
      Dimension2Id: selectedLevels[1] ? +selectedLevels[1].dimId : null,
      Level2Id: selectedLevels[1] ? +selectedLevels[1].levelId : null,
    
      Dimension3Id: selectedLevels[2] ? +selectedLevels[2].dimId : null,
      Level3Id: selectedLevels[2] ? +selectedLevels[2].levelId : null,
    
      Dimension4Id: selectedLevels[3] ? +selectedLevels[3].dimId : null,
      Level4Id: selectedLevels[3] ? +selectedLevels[3].levelId : null,
    } as ScopeFilterDto;
    this.historyService.getScopes(filter).subscribe({
      next: (response: ResponseDto<EnvironmentScopeDto[]>) => {
        this.scopes = response.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
    });
  }
  
  fetchScopesByEnvironment(): void {
    this.historyService.getScopesByEnvironment(this.selectedEnvironmentId).subscribe({
      next: (response: ResponseDto<EnvironmentScopeDto[]>) => {
        this.scopes = this.sortArrayBySortingValue(response.data);
        console.log(this.scopes)
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
    });
  }

  sortArrayBySortingValue(data: EnvironmentScopeDto[]): EnvironmentScopeDto[] {
    return data.sort((a, b) => {
      const sortingValuesA = a.sortingValue.split(";").map(value => parseFloat(value)).sort((x, y) => x - y);
      const sortingValuesB = b.sortingValue.split(";").map(value => parseFloat(value)).sort((x, y) => x - y);
  
      const length = Math.min(sortingValuesA.length, sortingValuesB.length);
  
      for (let i = 0; i < length; i++) {
        if (sortingValuesA[i] < sortingValuesB[i]) return -1;
        if (sortingValuesA[i] > sortingValuesB[i]) return 1;
      }
  
      if (sortingValuesA.length < sortingValuesB.length) return -1;
      if (sortingValuesA.length > sortingValuesB.length) return 1;
  
      return 0;
    });
  }
  
  fetchDataTypes(): void {
    this.factService.getDataTypes().subscribe({
      next: (response: ResponseDto<DataTypeDto[]>) => {
        this.types = response.data;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
    });
  }
  
  fetchEnvironments(): void {
    this.environmentService.getEnvironments().subscribe({
      next: (response: ResponseDto<EnvironmentDto[]>) => {
        this.environments = response.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching environments', err);
      }
    });
  }
  
  fetchDimensions(): void {
    this.dimensionService.getDimensions().subscribe({
      next: (response: ResponseDto<DimensionDto[]>) => {
        this.dimensions = response.data;
      },
      error: (err) => {
        console.error('Error fetching dimensions', err);
      }
    });
  }
  
  fetchSettings(): void {
    this.settingsService.getSettings().subscribe({
      next: (response: ResponseDto<SettingsDto[]>) => {
        this.settings = response.data;
      },
      error: (err) => {
        console.error('Error fetching settings', err);
      }
    });
  }

  fetchDimensionLevels(): void {
    this.levelService.getDimensionLevels().subscribe({
      next: result => {
        this.dimensionLevels = result.data;
        this.selectedLevels = this.dimensionLevels.reduce((acc, curr) => {
          if (curr.levels && curr.levels.length > 0) {
            return { ...acc, [curr.dimensionId]: curr.levels[0].id };
          }
          return acc;
        }, {});
      },
      error: (err) => {
        console.error('Error fetching dimension levels', err);
      }
    });
  }

  fetchScopeData(): void {
    this.historyService.getScopeDataTest(this.selectedScope!).subscribe({
      next: (response) => {
        this.scopeDataByDataType = response.data;
      },
      error: (err) => {
        console.error('Error fetching scope data', err);
      }
    });
  }

  fetchTimeAggregations(): void {
    let levelId: number = { "YEAR": 1, "SEMESTER": 2, "TRIMESTER": 3, "MONTH": 4, "WEEK": 5 }[this.selectedTimeAggregationLabel] || 1;

    this.historyService.getTimeAggregations(levelId).subscribe({
      next: (response: ResponseDto<TimeAggregationDto[]>) => {
        this.timeAggregationDtos = response.data.sort((a, b) => a.label.localeCompare(b.label));
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchTimeLevels() {
    this.levelService.getTimeLevels().subscribe({
      next: (response: ResponseDto<GetLevelDto[]>) => {
        this.timeLevels = response.data;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  getAmountForTypeAndTime(typeId: number, timeAggregationId: number): number | null {
    if (this.scopeDataByDataType[typeId] === undefined) return 0;
    const item = this.scopeDataByDataType[typeId].find(data => data.timeDimension.timeAggregationId === timeAggregationId);
    return item ? item.amount : 0;
  }

  onTimeAggregationLabelChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedTimeAggregationLabel = selectElement.value;
    this.fetchTimeAggregations();
    this.fetchScopeData();
  }

  onLevelChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedLevelId = +selectElement.value;
    this.selectedLevels[dimensionId] = selectedLevelId;
  }

  onEnvironmentChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedEnvironmentId = +selectElement.value;
  }

  onSelectScope(scope : EnvironmentScopeDto){
    this.selectedScope = scope;
    this.fetchTimeAggregations();
    this.fetchScopeData();
  }

  getEditKey(typeId: number, timeAggregationId: number) : string {
    return `${typeId}-${timeAggregationId}`;
  }

  editAmount(typeId: number, timeAggregationId: number): void {
    this.isEditing[this.getEditKey(typeId, timeAggregationId)] = true;
  }

  saveAmount(typeId: number, timeLabel: TimeAggregationDto, event: any): void {
    const newAmount = +event.target.value;
    if (newAmount !== this.getAmountForTypeAndTime(typeId, timeLabel.timeAggregationId)) {
      let scope : GetScopeDataDto | null = this.scopeDataByDataType[typeId].find(s => s.timeDimension.timeAggregationId == timeLabel.timeAggregationId) || null;
      if (scope == null) {
        // a refaire !!!!!!!!!
        const newFact = new FactCreateDto(
          newAmount, typeId, 
          timeLabel.timeAggregationId, 
          this.scopeDataByDataType[typeId][0].aggregationIds[0],
          this.scopeDataByDataType[typeId][0].aggregationIds[1] ?? null,
          this.scopeDataByDataType[typeId][0].aggregationIds[2] ?? null,
          this.scopeDataByDataType[typeId][0].aggregationIds[3] ?? null
        );
        this.factService.createFact(newFact).subscribe({
          next: (response) => {
            this.fetchScopeData();
          },
          error: (err) => {
            console.error('Error fetching levels', err);
          }
        });
      } else {
        this.factService.updateFact(new FactUpdateDto(scope.factId, typeId, newAmount)).subscribe({
          next: (response: boolean) => {
            if (response) scope!.amount = newAmount;
          },
          error: (err) => {
            console.error('Error fetching levels', err);
          }
        });
      }
    }
    this.isEditing[this.getEditKey(typeId, timeLabel.timeAggregationId)] = false;
  }

  cancelEdit(typeId: number, timeAggregationId: number): void {
    this.isEditing[this.getEditKey(typeId, timeAggregationId)] = false;
  }

  get uniqueTimeAggregationLabels() {
    return this.timeLevels.map(tl => tl.label);
  }
}
