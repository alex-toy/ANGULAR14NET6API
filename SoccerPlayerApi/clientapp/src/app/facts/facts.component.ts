import { Component } from '@angular/core';
import { FactService } from '../services/fact.service';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { GetAggregationsResultDto } from '../models/aggregations/getAggregationsResultDto';
import { MatDialog } from '@angular/material/dialog';
import { CreateFactComponent } from './create-fact/create-fact.component';
import { FactUpdateDto } from '../models/facts/factUpdateDto';
import { GetAggregationDto } from '../models/aggregations/getAggregationDto';
import { ResponseDto } from '../models/responseDto';
import { DataTypeDto } from '../models/facts/DataTypeDto';
import { FactDto } from '../models/facts/FactDto';

@Component({
  selector: 'app-facts',
  templateUrl: './facts.component.html',
  styleUrls: ['./facts.component.css']
})
export class FactsComponent {
  factsResult: FactDto[] = [];
  amount: number = 0;
  type: DataTypeDto = {id: 0, label: "" } as DataTypeDto;
  filter = new GetFactFilterDto(0, 0, 0, 0, undefined, undefined, 0, undefined, undefined, 0, undefined, undefined, undefined);
  factTypes : DataTypeDto[] = [];
  dimensions: DimensionDto[] = [];
  dimensionLevels: GetDimensionLevelDto[] = [];
  dimensionValues: { [dimensionId:number] : GetAggregationDto[]} = {};

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

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  updateFact(fact: FactDto){
    this.factService.updateFact(new FactUpdateDto(fact.id, this.type.id, this.amount)).subscribe({
      next: (isSuccess: boolean) => {
        if (isSuccess){
          console.log(isSuccess)
          const oldFact = this.factsResult.find(f => f.id === fact.id);
          if (oldFact !== undefined) oldFact.amount = fact.amount;
          if (oldFact !== undefined) oldFact.dataTypeId = fact.dataTypeId;
          this.fetchFacts();
          this.fetchFactTypes();
        }
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  fetchFacts(): void {
    this.factService.getFacts(this.filter).subscribe({
      next: (response: ResponseDto<FactDto[]>) => {
        this.factsResult = response.data;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
  
  fetchFactTypes(): void {
    this.factService.getFactTypes().subscribe({
      next: (response: ResponseDto<DataTypeDto[]>) => {
        this.factTypes = response.data;
      },
      error: (err) => {
        console.error('Error fetching scopes', err);
      }
    });
  }

  fetchDimensions(): void {
      this.dimensionService.getDimensions().subscribe({
        next: (response: ResponseDto<DimensionDto[]>) => {
          this.dimensions = response.data;
        },
        error: (err) => {
          console.error('Error fetching levels', err);
        }
      });
  }

  fetchDimensionValues(levelId: number, dimensionId: number): void {
    this.dimensionService.getDimensionValues(levelId).subscribe({
      next: (response: GetAggregationsResultDto) => {
        this.dimensionValues[dimensionId] = response.aggregations;
      },
      error: (err) => {
        console.error('Error fetching levels', err);
      }
    });
  }

  fetchDimensionLevels(): void {
    this.levelService.getDimensionLevels().subscribe({
      next: result => {
        this.dimensionLevels = result.data;
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
    console.log(this.selectedDimensionValues)
    // this.filter.aggregationIds = Object.values(this.selectedDimensionValues);
  }

  onfactTypeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.filter.dataTypeId = +selectElement.value;
  }
}
