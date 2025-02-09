import { ImportErrorDto } from './ImportErrorDto'; // Adjust the path as necessary

export class ImportFactCreateResultDto {
    linesCreatedCount: number;
    message: string;
    importErrors: ImportErrorDto[];

    constructor(linesCreatedCount: number, message: string, importErrors: ImportErrorDto[]) {
        this.linesCreatedCount = linesCreatedCount;
        this.message = message;
        this.importErrors = importErrors;
    }
}
