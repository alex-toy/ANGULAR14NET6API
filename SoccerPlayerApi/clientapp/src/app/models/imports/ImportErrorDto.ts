export class ImportErrorDto {
    rowNumber: number;
    description: string;

    constructor(rowNumber: number, description: string) {
        this.rowNumber = rowNumber;
        this.description = description;
    }
}
