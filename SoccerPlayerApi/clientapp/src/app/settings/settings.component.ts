import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { SettingsDto } from '../models/settings/settingsDto';
import { SettingCreateDto } from '../models/settings/settingCreateDto';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settings: SettingsDto[] = []; // Array to hold the settings data
  editableSettings: SettingsDto[] = []; // Array for storing editable settings
  newSetting: SettingCreateDto = { key: '', value: '' }; // Object for the new setting

  constructor(private settingsService: SettingsService) { }

  ngOnInit(): void {
    this.fetchSettings(); // Fetch settings when the component initializes
  }

  fetchSettings(): void {
    this.settingsService.getSettings().subscribe({
      next: (response) => {
        this.settings = response.data; // Store the settings data
        this.editableSettings = JSON.parse(JSON.stringify(this.settings)); // Create a copy of settings to allow editing
      },
      error: (err) => {
        console.error('Error fetching settings:', err); // Handle errors
      }
    });
  }
  
  updateSetting(setting: SettingsDto): void {
    this.settingsService.updateSetting(setting).subscribe({
      next: (response) => {
        const index = this.editableSettings.findIndex(s => s.id === setting.id);
        if (index !== -1) {
          this.editableSettings[index] = { ...setting }; // Replace the updated setting
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
}
