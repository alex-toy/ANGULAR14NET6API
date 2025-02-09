import { ImportErrorDto } from './ImportErrorDto'; // Adjust the path as necessary

export class ImportFactCreateResultDto {
    LinesCreatedCount: number;
    Message: string;
    ImportErrors: ImportErrorDto[];

    constructor(linesCreatedCount: number, message: string, importErrors: ImportErrorDto[]) {
        this.LinesCreatedCount = linesCreatedCount;
        this.Message = message;
        this.ImportErrors = importErrors;
    }
}
