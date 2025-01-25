import { Component } from '@angular/core';
import { FactService } from '../services/fact.service';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { LevelService } from '../services/level.service';
import { DimensionsService } from '../services/dimensions.service';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { GetDimensionLevelDto } from '../models/levels/getDimensionLevelDto';
import { GetAggregationsResultDto } from '../models/aggregations/getAggregationsResultDto';
import { GetFactTypesResultDto } from '../models/facts/getFactTypesResultDto';
import { MatDialog } from '@angular/material/dialog';
import { CreateFactComponent } from './create-fact/create-fact.component';
import { GetFactResultDto } from '../models/facts/getFactResultDto';
import { FactUpdateDto } from '../models/facts/factUpdateDto';
import { GetAggregationDto } from '../models/aggregations/getAggregationDto';
import { ResponseDto } from '../models/responseDto';
import { TypeDto } from '../models/facts/typeDto';

@Component({
  selector: 'app-facts',
  templateUrl: './facts.component.html',
  styleUrls: ['./facts.component.css']
})
export class FactsComponent {
  factsResult: GetFactsResultDto | null = null;
  amount: number = 0;
  type: TypeDto = {id: 0, label: "" } as TypeDto;
  filter: GetFactFilterDto = {
    type: '',
    factDimensionFilters: [],
    aggregationIds: []
  };
  
  factTypes : TypeDto[] = [];
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

    // Optionally, handle actions when the modal is closed
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  updateFact(fact: GetFactResultDto){
    this.factService.updateFact(new FactUpdateDto(fact.id, this.type.id, this.amount)).subscribe({
      next: (isSuccess: boolean) => {
        if (isSuccess){
          console.log(isSuccess)
          const oldFact = this.factsResult?.facts.find(f => f.id === fact.id);
          if (oldFact !== undefined) oldFact.amount = fact.amount;
          if (oldFact !== undefined) oldFact.type = fact.type;
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
      next: (response: ResponseDto<TypeDto[]>) => {
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
    this.filter.aggregationIds = Object.values(this.selectedDimensionValues);
  }

  onfactTypeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.filter.type = selectElement.value;
  }
}
