import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { DimensionDto } from 'src/app/models/dimensions/dimensionDto';
import { GetAggregationDto } from 'src/app/models/aggregations/getAggregationDto';
import { GetAggregationsResultDto } from 'src/app/models/aggregations/getAggregationsResultDto';
import { FactCreateDto } from 'src/app/models/facts/factCreateDto';
import { GetDimensionLevelDto } from 'src/app/models/levels/getDimensionLevelDto';
import { DimensionsService } from 'src/app/services/dimensions.service';
import { FactService } from 'src/app/services/fact.service';
import { LevelService } from 'src/app/services/level.service';
import { ResponseDto } from 'src/app/models/responseDto';
import { DataTypeDto } from 'src/app/models/facts/DataTypeDto';

@Component({
  selector: 'app-create-fact',
  templateUrl: './create-fact.component.html',
  styleUrls: ['./create-fact.component.css']
})
export class CreateFactComponent {

  newFact = new FactCreateDto(0, 0, 0, 0, undefined, undefined, 0);

  factTypes : DataTypeDto[] = [];
  dimensions: DimensionDto[] = [];
  dimensionLevels: GetDimensionLevelDto[] = [];
  aggregations: { [dimensionId:number] : GetAggregationDto[]} = {};

  selectedType: string = "";
  selectedLevels: { [dimensionId:number] : number} = {};
  selectedDimensionValues: { [dimensionId:number] : number} = {};

  constructor(
      private factService: FactService, 
      private levelService: LevelService, 
      private dimensionService: DimensionsService, 
      public dialogRef: MatDialogRef<CreateFactComponent>) {}

  ngOnInit(): void {
    this.fetchFactTypes();
    this.fetchDimensions();
    this.fetchDimensionLevels();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  
  createFact(): void {
    this.factService.createFact(this.newFact).subscribe({
      next: (data) => {
        console.log(data)
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
        console.error(err);
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
        this.aggregations[dimensionId] = response.aggregations;
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
    const myArray = Object.keys(this.selectedDimensionValues).map(key => ({ key: key, value: this.selectedDimensionValues[+key] }));
    // this.newFact.Dimension1AggregationId = this.selectedDimensionValues.AggregationIds[0]
    
    // aggregationIds = Object.values(this.selectedDimensionValues);
  }

  onfactTypeChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.newFact.DataTypeId = +selectElement.value;
  }

  onSave(): void {
    // this.newFact.aggregationIds = Object.values(this.selectedDimensionValues);
    // this.createFact();
    // this.dialogRef.close();
  }
}
