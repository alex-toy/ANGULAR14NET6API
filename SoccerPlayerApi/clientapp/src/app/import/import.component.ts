import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import * as XLSX from 'xlsx';
import { ImportFactDto } from '../models/imports/ImportFactDto';
import { ImportService } from '../services/import.service';
import { ImportErrorDto } from '../models/imports/ImportErrorDto';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.css']
})
export class ImportComponent {
  file: File | null = null;
  errorMessage: string = '';
  successMessage: string = '';
  importErrors: ImportErrorDto[] = [];
  linesCreatedCount: number = 0;
  message: string = '';
  isSuccess: boolean = false;

  constructor(
    private http: HttpClient,
    private importService: ImportService
  ) {}

  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file && file.name.endsWith('.xlsx')) {
      this.file = file;
      this.errorMessage = ''; // Reset error message if a valid file is selected
    } else {
      this.errorMessage = 'Please select a valid Excel file (.xlsx)';
      this.file = null;
    }
  }

  onSubmit(): void {
    if (!this.file) {
      this.errorMessage = 'Please upload an Excel file first.';
      return;
    }

    const reader = new FileReader();
    reader.onload = (e: any) => {
      const data = e.target.result;
      const workbook = XLSX.read(data, { type: 'binary' });

      const sheetName = workbook.SheetNames[0];
      const worksheet = workbook.Sheets[sheetName];

      const jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
      
      const formattedData = jsonData.slice(1).map((row: any, index: number) => ({
        RowNumber: index + 2,
        Amount: row[0],
        DataType: row[1],
        Dimension1Aggregation: row[2],
        Dimension2Aggregation: row[3],
        Dimension3Aggregation: row[4],
        Dimension4Aggregation: row[5],
        TimeAggregation: row[6]
      } as ImportFactDto));
      
      this.importService.createImportFact(formattedData).subscribe({
        next: (response) => {
          this.linesCreatedCount = response.data.linesCreatedCount;
          this.message = response.data.message;
          this.importErrors = response.data.importErrors;
          this.isSuccess = response.isSuccess;

          if (this.isSuccess) {
            this.successMessage = 'File imported successfully!';
            this.errorMessage = '';
          } else {
            this.errorMessage = 'Errors occurred during import.';
            this.successMessage = '';
          }
        },
        error: (err) => {
          console.error('Error createImportFact:', err);
        }
      });

    };
    reader.readAsBinaryString(this.file);
  }
}
