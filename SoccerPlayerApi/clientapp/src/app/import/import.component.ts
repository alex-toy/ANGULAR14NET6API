import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.css']
})
export class ImportComponent {
  file: File | null = null;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private http: HttpClient) {}

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
      
      const formattedData = jsonData.slice(1).map((row: any) => ({
        Fact: row[0],
        DataType: row[1],
        Dimension1Aggregation: row[2],
        Dimension2Aggregation: row[3],
        TimeAggregation: row[4]
      }));

      // Send the data to the backend
      // this.uploadData(formattedData);

      console.log(formattedData)

    };
    reader.readAsBinaryString(this.file);
  }

  uploadData(data: any): void {
    this.http.post(`${environment.apiUrl}/upload`, data).subscribe({
      next: (response) => {
        this.successMessage = 'File uploaded successfully!';
        this.errorMessage = ''; // Clear any previous error messages
      },
      error: (error) => {
        this.errorMessage = 'Error uploading file. Please try again later.';
        this.successMessage = ''; // Clear success message if error occurs
      }
    });
  }
}
