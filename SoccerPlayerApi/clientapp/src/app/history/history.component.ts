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
import { TypeDto } from '../models/facts/typeDto';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  settings: SettingsDto[] = [];
  isEditing: { [key: string]: boolean } = {};

  scopes: ScopeDto[] = [];
  selectedScope: ScopeDto | null = null; // The selected scope
  scopeData: GetScopeDataDto[] = [];
  types: TypeDto[] = [];

  filterMode: string = 'dimensions';
  isLoading: boolean = true;
  selectedTimeLabel = 'YEAR';
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
    this.fetchSettings();
    this.fetchScopes();
    this.fetchDimensions();
    this.fetchDimensionLevels();
    this.fetchEnvironments();
    this.fetchFactTypes();
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
      next: (response: ResponseDto<ScopeDto[]>) => {
        this.scopes = response.data.map(d => new ScopeDto(d));
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
    });
  }
  
  fetchFactTypes(): void {
    this.factService.getFactTypes().subscribe({
      next: (response: ResponseDto<TypeDto[]>) => {
        this.types = response.data;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
    });
  }
  
  fetchScopesByEnvironment(): void {
    this.historyService.getScopesByEnvironment(this.selectedEnvironmentId).subscribe({
      next: (response: ResponseDto<ScopeDto[]>) => {
        this.scopes = response.data.map(d => new ScopeDto(d));
        this.isLoading = false;
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
    let levelId : number = 0;
    console.log(this.selectedTimeAggregationLabel)
    switch ( this.selectedTimeAggregationLabel ) {
      case "MONTH":
        levelId = 4;
        break;
      case "WEEK":
        levelId = 5;
        break;
      default:
        levelId = 1;
    }

    this.historyService.getTimeAggregations(levelId).subscribe({
      next: (response: ResponseDto<TimeAggregationDto[]>) => {
        this.timeAggregationDtos = response.data.sort((a, b) => a.label.localeCompare(b.label));;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  get uniqueTimeAggregationLabels() {
    // return this.timeAggregationDtos.map(x => x.label);
    return ["MONTH", "WEEK"];
  }

  getAmountForTypeAndYear(typeId: number, year: number): number | null {
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

  onSelectScope(scope : ScopeDto){
    this.selectedScope = scope;
    this.fetchTimeAggregations();
  }

  getEditKey(typeId: number, timeAggregationId: number) : string {
    return `${typeId}-${timeAggregationId}`;
  }

  editAmount(typeId: number, timeAggregationId: number): void {
    this.isEditing[this.getEditKey(typeId, timeAggregationId)] = true;
  }

  saveAmount(typeId: number, timeLabel: TimeAggregationDto, event: any): void {
    const newAmount = +event.target.value;
    if (newAmount !== this.getAmountForTypeAndYear(typeId, timeLabel.timeAggregationId)) {
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
}
