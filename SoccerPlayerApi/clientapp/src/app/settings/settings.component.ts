import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { SettingsDto } from '../models/settings/settingsDto';
import { SettingCreateDto } from '../models/settings/settingCreateDto';
import { DimensionsService } from '../services/dimensions.service';
import { DimensionDto } from '../models/dimensions/dimensionDto';
import { CreateLevelDto } from '../models/levels/createLevelDto';
import { LevelService } from '../services/level.service';
import { GetLevelDto } from '../models/levels/getLevelDto';

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
  newDimension: DimensionDto = { id: 0, value: '', levels: [] };

  newLevel: CreateLevelDto = { value: '', dimensionId: 0, ancestorId: null };
  showLevelFormForDimension: number | null = null;

  constructor(
    private settingsService: SettingsService,
    private dimensionsService: DimensionsService,
    private levelService: LevelService,
  ) { }

  ngOnInit(): void {
    this.fetchSettings(); 
    this.fetchDimensions();
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

  fetchDimensions(): void {
    this.dimensionsService.getDimensions().subscribe(
      (response) => {
        if (response.isSuccess) {
          this.dimensions = response.data;

          // Fetch levels for each dimension using LevelService
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
    if (this.newDimension.value) {
      this.dimensionsService.createDimension(this.newDimension).subscribe(
        response => {
          this.newDimension = { id: 0, value: '', levels: [] };
          this.fetchDimensions();
        },
        error => {
          console.error('Error creating dimension:', error);
        }
      );
    }
  }

  addLevel(dimensionId: number): void {
    this.showLevelFormForDimension = dimensionId;  // Show modal for this dimension
  }

  onLevelAdded(newLevel: CreateLevelDto): void {
    if (this.showLevelFormForDimension !== null) {
      const dimension = this.dimensions.find(d => d.id === this.showLevelFormForDimension);
      if (dimension) {
        dimension.levels.push(newLevel as GetLevelDto);
        dimension.levels.sort((a, b) => a.id - b.id);  // Sort levels by ID (created order)
      }
    }
  }

  onCloseLevelModal(): void {
    this.showLevelFormForDimension = null;  // Close the modal
  }
}
