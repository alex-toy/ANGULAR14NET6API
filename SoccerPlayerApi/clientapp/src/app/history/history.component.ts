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

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  settings: SettingsDto[] = [];
  scopes: ScopeDto[] = [];
  selectedScope: ScopeDto | null = null; // The selected scope
  scopeData: GetScopeDataDto[] = [];
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
  ) {}

  ngOnInit(): void {
    this.fetchSettings();
    this.fetchScopes();
    this.fetchDimensions();
    this.fetchDimensionLevels();
    this.fetchEnvironments();
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
    this.historyService.getScopeData(scope).subscribe({
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

  get uniqueTypes() {
    const types = this.scopeData
      .filter(item => item.timeDimension.timeAggregationLabel === this.selectedTimeAggregationLabel)
      .map(item => item.type);
    return [...new Set(types)];
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

  getMonthsBetweenDates(): string[] {
    const presentDate = this.settings.find(x => x.key == "PresentDate")?.value!;
    const pastSpan : number = +this.settings.find(x => x.key == "PastSpan")?.value!;
    let date = new Date(presentDate);
    date.setMonth(date.getMonth() - pastSpan);
    const startDate = date.toISOString().split('T')[0];

    let start = new Date(startDate);
    let end = new Date(presentDate);
    
    let months: string[] = [];
    
    while (start <= end) {
        months.push(start.toISOString().split('T')[0]);
        start.setMonth(start.getMonth() + 1);
    }
    
    // return months;
    return ["2022", "2023", "2024", "2025"];
  }
}
