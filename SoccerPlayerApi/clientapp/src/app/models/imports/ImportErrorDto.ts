export class ImportErrorDto {
    RowNumber: number;
    Description: string;

    constructor(rowNumber: number, description: string) {
        this.RowNumber = rowNumber;
        this.Description = description;
    }
}
