import { Component } from '@angular/core';
import { FactService } from '../services/fact.service';
import { GetFactsResultDto } from '../models/facts/getFactsResultDto';
import { GetFactFilterDto } from '../models/facts/GetFactFilterDto';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { LevelService } from '../services/level.service';
import { GetLevelsResultDto } from '../models/levels/getLevelsResultDto';
import { DimensionsService } from '../services/dimensions.service';
import { GetDimensionsResultDto } from '../models/dimensions/getDimensionsResultDto';
import { DimensionDto } from '../models/dimensions/dimensionDto';

@Component({
  selector: 'app-facts',
  templateUrl: './facts.component.html',
  styleUrls: ['./facts.component.css']
})
export class FactsComponent {
  factsResult: GetFactsResultDto | null = null;
  errorMessage: string = '';
  filter: GetFactFilterDto = {
    type: '',
    factDimensionFilters: [],
    dimensionValueIds: []
  };
  levels: GetLevelDto[] = [];
  dimensionLevels: { [dimensionId: number]: GetLevelDto[] } = {};
  dimensions: DimensionDto[] = [];
  selectedLevel: number = 1;

  constructor(private factService: FactService, private levelService: LevelService, private dimensionService: DimensionsService) {}

  ngOnInit(): void {
    this.fetchDimensions();
    this.fetchLevels();
    this.fetchFacts();
  }

  fetchFacts(): void {
    this.factService.getFacts(this.filter).subscribe({
      next: (data: GetFactsResultDto) => {
        this.factsResult = data;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load facts';
        console.error(err);
      }
    });
  }

  applyFilter(): void {
    this.filter.type = 'sales';  
    this.filter.dimensionValueIds = [2, 3, 4];
    this.fetchFacts();
  }

  fetchLevels(): void {
    const dimensionId = 1;
    this.levelService.getLevels(dimensionId).subscribe({
      next: (response: GetLevelsResultDto) => {
        this.levels = response.levels;
        if (this.levels.length > 0) this.selectedLevel = this.levels[0].id;

        this.dimensionLevels[dimensionId] = response.levels;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load levels. Please try again later.';
        console.error('Error fetching levels', err);
      }
    });
  }

  onLevelChange(): void {
    console.log('Selected level:', this.selectedLevel);
    this.fetchFacts();
  }

  fetchDimensions(): void {
    this.dimensionService.getDimensions().subscribe({
      next: (response: GetDimensionsResultDto) => {
        this.dimensions = response.dimensions;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load dimensions. Please try again later.';
        console.error('Error fetching levels', err);
      }
    });
  }
}
