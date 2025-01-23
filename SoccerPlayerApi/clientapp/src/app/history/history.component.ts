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
  types: string[] = [];

  filterMode: string = 'dimensions';
  isLoading: boolean = true;
  selectedTimeLabel = 'YEAR';
  selectedTimeAggregationLabel: string = 'YEAR';
  
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
      next: (response: ResponseDto<string[]>) => {
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
    this.fetchScopeData();
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

  get timeAggregationLabel(){

    let presentDate : string = "";
    let pastSpan : number = 0;
    this.settingsService.getHistoryDatesMonth(presentDate, pastSpan)
    
    switch ( this.selectedTimeAggregationLabel ) {
      case "MONTH":
          presentDate = this.settings.find(x => x.key == "PresentMonth")?.value!;
          pastSpan = +this.settings.find(x => x.key == "PastSpan")?.value!;
          return this.settingsService.getHistoryDatesMonth(presentDate, pastSpan);
      case "WEEK":
          presentDate = this.settings.find(x => x.key == "PresentWeek")?.value!;
          pastSpan = +this.settings.find(x => x.key == "PastSpan")?.value!;
          return this.settingsService.getHistoryDatesWeek(presentDate, pastSpan);
      default:
          presentDate = this.settings.find(x => x.key == "PresentDate")?.value!;
          pastSpan = +this.settings.find(x => x.key == "PastSpan")?.value!;
          return this.settingsService.getHistoryDatesMonth(presentDate, pastSpan);
    }
  }

  get uniqueYears() {
    const years = this.scopeData
      .filter(item => item.timeDimension.timeAggregationLabel === this.selectedTimeAggregationLabel)
      .map(item => item.timeDimension.timeAggregationValue);
    return [...new Set(years)];
  }

  get sortedYears() {
    const years = this.scopeData
      .filter(item => item.timeDimension.timeAggregationLabel === this.selectedTimeAggregationLabel)
      .map(item => item.timeDimension.timeAggregationValue);
    return [...new Set(years)].sort((a, b) => parseInt(a) - parseInt(b));
  }

  get uniqueTimeAggregationLabels() {
    // const labels = this.scopeData
    //   .map(item => item.timeDimension.timeAggregationLabel);
    // return [...new Set(labels)];
    return ["MONTH", "WEEK"];
  }

  getAmountForTypeAndYear(type: string, year: string): number | null {
    const item = this.scopeData.find(
      (data) => data.type === type && data.timeDimension.timeAggregationValue === year
    );
    return item ? item.amount : null;
  }

  onTimeAggregationLabelChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedTimeAggregationLabel = selectElement.value;
  }

  editAmount(type: string, year: string): void {
    this.isEditing[`${type}-${year}`] = true;
  }

  saveAmount(type: string, timeLabel: string, event: any): void {
    const newAmount = +event.target.value; // Parse the new value as a number
    if (newAmount !== this.getAmountForTypeAndYear(type, timeLabel)) {
      let scope : GetScopeDataDto | null = this.scopeData.find(s => s.timeDimension.timeAggregationValue == timeLabel && s.type == type) || null;
      if (scope == null) {
        this.factService.createFact(new FactCreateDto(type, newAmount, this.scopeData[0].aggregationIds, timeLabel)).subscribe({
          next: (response: FactCreateResultDto) => {
            this.fetchScopeData();
          },
          error: (err) => {
            console.error('Error fetching levels', err);
          }
        });
      } else {
        this.factService.updateFact(new FactUpdateDto(scope.factId, type, newAmount)).subscribe({
          next: (response: boolean) => {
            if (response) scope!.amount = newAmount;
          },
          error: (err) => {
            console.error('Error fetching levels', err);
          }
        });
      }
    }
    this.isEditing[`${type}-${timeLabel}`] = false; // End editing mode
  }

  // Method to cancel the edit
  cancelEdit(type: string, year: string): void {
    this.isEditing[`${type}-${year}`] = false;
  }
}
