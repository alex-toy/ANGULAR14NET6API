import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { SettingsDto } from '../models/settings/settingsDto';
import { SettingCreateDto } from '../models/settings/settingCreateDto';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { CreateLevelDto } from '../models/levels/createLevelDto';
import { LevelService } from '../services/level.service';
import { GetLevelDto } from '../models/levels/getLevelDto';
import { DataTypeDto } from '../models/facts/DataTypeDto';
import { FactService } from '../services/fact.service';
import { AggregationCreateDto } from '../models/aggregations/aggregationCreateDto';
import { AggregationService } from '../services/aggregation.service';
import { GetAggregationDto } from '../models/aggregations/getAggregationDto';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settings: SettingsDto[] = [];
  editableSettings: SettingsDto[] = [];
  newSetting: SettingCreateDto = { key: '', value: '' };

  dimensions: DimensionDto[] = [];
  newDimension: DimensionDto = { id: 0, label: '', levels: [] };

  newLevel: CreateLevelDto = { label: '', dimensionId: 0, ancestorId: null };
  showLevelFormForDimension: number | null = null;
  ancestorId: number | null = null;

  newType: DataTypeDto = { id: 0, label: '' } as DataTypeDto;
  types: DataTypeDto[] = [];
  
  newAggregation: AggregationCreateDto = { levelId: 0, motherAggregationId: 0, value: '' };
  aggregations: GetAggregationDto[] = [];

  constructor(
    private settingsService: SettingsService,
    private dimensionsService: DimensionsService,
    private aggregationService: AggregationService,
    private levelService: LevelService,
    private factService: FactService
  ) { }

  ngOnInit(): void {
    this.fetchSettings(); 
    this.fetchDimensions();
    this.fetchDataTypes();
  }

  fetchSettings(): void {
    this.settingsService.getSettings().subscribe({
      next: (response) => {
        this.settings = response.data;
        this.editableSettings = JSON.parse(JSON.stringify(this.settings));
      },
      error: (err) => {
        console.error('Error fetching settings:', err);
      }
    });
  }

  fetchLevels(dimensionId : number): void {
    this.levelService.getLevels(dimensionId).subscribe({
      next: (response) => {
        let dimension = this.dimensions.find(x => x.id === dimensionId)!;
        dimension.levels = response.data;
      },
      error: (err) => {
        console.error('Error fetching settings:', err);
      }
    });
  }

  updateSetting(setting: SettingsDto): void {
    this.settingsService.updateSetting(setting).subscribe({
      next: (response) => {
        const index = this.editableSettings.findIndex(s => s.id === setting.id);
        if (index !== -1) {
          this.editableSettings[index] = { ...setting };
        }
      },
      error: (err) => {
        console.error('Error updating setting:', err);
      }
    });
  }

  addSetting(): void {
    if (!this.newSetting.key || !this.newSetting.value) return;

    this.settingsService.addSetting(this.newSetting).subscribe({
      next: (response) => {
        this.settings.push(this.newSetting as SettingsDto);
        this.editableSettings.push(this.newSetting as SettingsDto);
        this.newSetting = { key: '', value: '' };
      },
      error: (err) => {
        console.error('Error adding new setting:', err);
      }
    });
  }

  setAncestorId(dimensionId: number){
    let levels = this.dimensions.find(x => x.id == dimensionId)!.levels;
    let higherLevel = levels.find(x => x.ancestorId == null);
    if (higherLevel === undefined) return;
    let ancestorId = higherLevel!.id;
    let sonLevel = levels.find(x => x.ancestorId == higherLevel!.id);

    while (sonLevel != undefined){
      ancestorId = higherLevel!.id;
      higherLevel = sonLevel;
      sonLevel = levels.find(x => x.ancestorId === ancestorId);
    }

    this.ancestorId = higherLevel.id;
  }

  fetchDimensions(): void {
    this.dimensionsService.getDimensions().subscribe(
      (response) => {
        if (response.isSuccess) {
          this.dimensions = response.data;

          this.dimensions.forEach(dimension => {
            this.levelService.getLevels(dimension.id).subscribe(levelResponse => {
              dimension.levels = levelResponse.data;
            });
          });
        } else {
          console.error('Failed to fetch dimensions');
        }
      },
      (error) => {
        console.error('Error fetching dimensions:', error);
      }
    );
  }

  addDimension(): void {
    if (this.newDimension.label) {
      this.dimensionsService.createDimension(this.newDimension).subscribe(
        response => {
          this.newDimension = { id: 0, label: '', levels: [] };
          this.fetchDimensions();
        },
        error => {
          console.error('Error creating dimension:', error);
        }
      );
    }
  }

  addLevel(dimensionId: number): void {
    this.showLevelFormForDimension = dimensionId;
    console.log(this.showLevelFormForDimension)
    
    let levels = this.dimensions.find(x => x.id == dimensionId)!.levels;
    let higherLevel = levels.find(x => x.ancestorId == null);
    if (higherLevel === undefined) return;
    let ancestorId = higherLevel!.id;
    let sonLevel = levels.find(x => x.ancestorId == higherLevel!.id);
    
    while (sonLevel != undefined){
      ancestorId = higherLevel!.id;
      higherLevel = sonLevel;
      sonLevel = levels.find(x => x.ancestorId === ancestorId);
    }

    this.ancestorId = higherLevel.id;
  }

  onLevelAdded(newLevel: CreateLevelDto): void {
    if (this.showLevelFormForDimension !== null) {
      const dimension = this.dimensions.find(d => d.id === this.showLevelFormForDimension);
      if (dimension) {
        dimension.levels.push(newLevel as GetLevelDto);
        dimension.levels.sort((a, b) => a.id - b.id);
      }
      this.fetchLevels(this.showLevelFormForDimension);
    }
  }

  onCloseLevelModal(): void {
    this.showLevelFormForDimension = null;
  }

  fetchDataTypes(): void {
    this.factService.getDataTypes().subscribe({
      next: (response) => {
        this.types = response.data;
      },
      error: (err) => {
        console.error('Error fetching types:', err);
      }
    });
  }

  addType(): void {
    if (!this.newType.label) return;

    this.factService.createType(this.newType).subscribe({
      next: (response) => {
        this.types.push(response.data);
        this.newType = { id: 0, label: '' } as DataTypeDto;
      },
      error: (err) => {
        console.error('Error adding new type:', err);
      }
    });
  }
}
