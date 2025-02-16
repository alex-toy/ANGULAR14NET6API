import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FrameCreateDto } from 'src/app/models/environments/environmentCreateDto';
import { GetDimensionLevelDto } from 'src/app/models/levels/getDimensionLevelDto';
import { EnvironmentService } from 'src/app/services/environment.service';
import { LevelService } from 'src/app/services/level.service';
import { EnvironmentSortingDto } from 'src/app/models/environments/environmentSortingDto'; // Import the new DTO
import { FactService } from 'src/app/services/fact.service';
import { DataTypeDto } from 'src/app/models/facts/DataTypeDto';
import { HistoryService } from 'src/app/services/history.service';
import { TimeAggregationDto } from 'src/app/models/facts/timeAggregationDto';
import { ResponseDto } from 'src/app/models/responseDto';
import { GetLevelDto } from 'src/app/models/levels/getLevelDto';

@Component({
  selector: 'app-create-environment',
  templateUrl: './create-environment.component.html',
  styleUrls: ['./create-environment.component.css']
})
export class CreateEnvironmentComponent {
  dimensionLevels: GetDimensionLevelDto[] = [];
  selectedLevels: { [dimensionId: number]: number } = {};
  dataTypes: DataTypeDto[] = [];
  selectedTimeLevelId: number = 0;
  timeAggregations : TimeAggregationDto[] = [];
  timeLevels : GetLevelDto[] = [];
  selectedTimeLevel : GetLevelDto = { id : 0, dimensionId : 0, label : "" };

  environment: FrameCreateDto = {
    name: '',
    description: '',
    dimension1Id: 0,
    levelIdFilter1: 0,
    dimension2Id: null,
    levelIdFilter2: null,
    dimension3Id: null,
    levelIdFilter3: null,
    dimension4Id: null,
    levelIdFilter4: null,
    frameSortings: []
  };

  newSorting: EnvironmentSortingDto = {
    environmentId: 0,
    orderIndex: 0,
    aggregator: 0,
    startTimeSpan: 0,
    endTimeSpan: 0,
    isAscending: 0,
    timeSpanBase: 0,
    dataTypeId: 0,
    timeLevelId: 0
  };

  // New EnvironmentSortingDto
  environmentSorting: EnvironmentSortingDto = {
    environmentId: 0,
    orderIndex: 0,
    aggregator: 0,     // Default SUM
    startTimeSpan: 0,
    endTimeSpan: 0,
    isAscending: 1,    // Default Ascending
    timeSpanBase: 0,    // Default History
    dataTypeId: 0,
    timeLevelId: 0
  };

  constructor(
    private environmentService: EnvironmentService,
    private levelService: LevelService,
    private historyService: HistoryService,
    private factService: FactService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.fetchTimeLevels();
    this.fetchDimensionLevels();
    this.fetchDataTypes();
    this.fetchTimeAggregations();
  }

  fetchDimensionLevels(): void {
    this.levelService.getDimensionLevels().subscribe({
      next: result => {
        this.dimensionLevels = result.data;
        this.selectedLevels = this.dimensionLevels.reduce((acc, curr) => {
          return { ...acc, [curr.dimensionId]: curr.levels[0].id };
        }, {});
      },
      error: (err) => {
        console.error('Error fetching dimension levels', err);
      }
    });
  }

  fetchDataTypes(): void {
    this.factService.getDataTypes().subscribe({
      next: (response) => {
        this.dataTypes = response.data;
      },
      error: (err) => {
        console.error('Error fetching types:', err);
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

  fetchTimeAggregations(): void {
    this.historyService.getTimeAggregations(+this.selectedTimeLevelId || 1).subscribe({
      next: (response: ResponseDto<TimeAggregationDto[]>) => {
        this.timeAggregations = response.data.sort((a, b) => a.label.localeCompare(b.label));;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  onLevelChange(dimensionId: number, event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedLevelId = +selectElement.value;
    this.selectedLevels[dimensionId] = selectedLevelId;
  }

  onTimeAggregationLabelChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectedTimeLevelId = +selectElement.value;
    this.fetchTimeAggregations();
  }

  onSubmit(): void {
    let dimensionCounter = 1;
    for (const [dimensionId, levelId] of Object.entries(this.selectedLevels)) {
      if (dimensionCounter == 1) {
        this.environment.dimension1Id = +dimensionId;
        this.environment.levelIdFilter1 = levelId;
      }

      if (dimensionCounter == 2) {
        this.environment.dimension2Id = +dimensionId;
        this.environment.levelIdFilter2 = levelId;
      }

      if (dimensionCounter == 3) {
        this.environment.dimension3Id = +dimensionId;
        this.environment.levelIdFilter3 = levelId;
      }

      if (dimensionCounter == 4) {
        this.environment.dimension4Id = +dimensionId;
        this.environment.levelIdFilter4 = levelId;
      }

      dimensionCounter++;
    }

    this.environmentService.createEnvironment(this.environment).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.environmentSorting.environmentId = response.data;
          this.router.navigate(['/environments']);
        } else {
          console.log(response.message);
        }
      },
      error: (err) => {
        console.error('Error creating environment:', err);
      }
    });
  }

  addSorting(): void {
    const maxOrderIndex = this.environment.frameSortings.length > 0
        ? Math.max(...this.environment.frameSortings.map(x => x.orderIndex))
        : 0;
    this.newSorting.orderIndex = maxOrderIndex + 1;
    this.environment.frameSortings.push({ ...this.newSorting });
    this.resetSortingForm();
  }

  // Reset the sorting form fields
  resetSortingForm(): void {
    this.newSorting = {
      environmentId: 0,
      orderIndex: 0,
      aggregator: 0,
      startTimeSpan: 0,
      endTimeSpan: 0,
      isAscending: 0,
      timeSpanBase: 0,
      dataTypeId : 0,
      timeLevelId: 0
    };
  }

  // Remove a sorting configuration by its index
  removeSorting(index: number): void {
    this.environment.frameSortings.splice(index, 1);
  }
}
