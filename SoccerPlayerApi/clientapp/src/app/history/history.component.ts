import { Component } from '@angular/core';
import { ScopeDto } from '../models/scopes/scopeDto';
import { HistoryService } from '../services/history.service';
import { ResponseDto } from '../models/responseDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { ScopeFilterDto } from '../models/scopes/scopeFilterDto';
import { ScopeDimensionFilterDto } from '../models/scopes/scopeDimensionFilterDto';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';
import { EnvironmentDto } from '../models/environments/environmentDto';
import { EnvironmentService } from '../services/environment.service';
import { SettingsService } from '../services/settings.service';
import { SettingsDto } from '../models/settings/settingsDto';
import { FactService } from '../services/fact.service';
import { FactCreateDto } from '../models/facts/factCreateDto';
import { FactCreateResultDto } from '../models/facts/factCreateResultDto';
import { FactUpdateDto } from '../models/facts/factUpdateDto';
import { TimeAggregationDto } from '../models/facts/timeAggregationDto';
import { DataTypeDto } from '../models/facts/typeDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { EnvironmentScopeDto } from '../models/scopes/environmentScopeDto';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  settings: SettingsDto[] = [];
  isEditing: { [key: string]: boolean } = {};

  scopes: EnvironmentScopeDto[] = [];
  selectedScope: EnvironmentScopeDto | null = null; // The selected scope
  scopeData: GetScopeDataDto[] = [];
  types: DataTypeDto[] = [];

  filterMode: string = 'dimensions';
  isLoading: boolean = true;

  selectedTimeLabel = 'YEAR';
  timeLevels : GetLevelDto[] = [];
  selectedTimeLevel : GetLevelDto = { id : 0, dimensionId : 0, value : "" };

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
    this.fetchScopes();
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
    let filter = {
      scopeDimensionFilters : Object.entries(this.selectedLevels).map(x => new ScopeDimensionFilterDto(+x[0], x[1]))
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
        this.scopes = response.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
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
    console.log(this.selectedScope)
    this.historyService.getScopeData(this.selectedScope!).subscribe({
      next: (response: ResponseDto<GetScopeDataDto[]>) => {
        this.scopeData = response.data;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchTimeAggregations(): void {
    let levelId: number = { "YEAR": 1, "SEMESTER": 2, "TRIMESTER": 3, "MONTH": 4, "WEEK": 5 }[this.selectedTimeAggregationLabel] || 1;

    this.historyService.getTimeAggregations(levelId).subscribe({
      next: (response: ResponseDto<TimeAggregationDto[]>) => {
        this.timeAggregationDtos = response.data.sort((a, b) => a.label.localeCompare(b.label));
        console.log(this.timeAggregationDtos)
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

  getAmountForTypeAndTime(typeId: number, year: number): number | null {
    const item = this.scopeData.find(data => data.typeId === typeId && data.timeDimension.timeAggregationId === year);
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
      let scope : GetScopeDataDto | null = this.scopeData.find(s => s.timeDimension.timeAggregationId == timeLabel.timeAggregationId && s.typeId == typeId) || null;
      if (scope == null) {
        this.factService.createFact(new FactCreateDto(typeId, newAmount, this.scopeData[0].aggregationIds, timeLabel.timeAggregationId)).subscribe({
          next: (response: FactCreateResultDto) => {
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
    return this.timeLevels.map(tl => tl.value);
  }
}
