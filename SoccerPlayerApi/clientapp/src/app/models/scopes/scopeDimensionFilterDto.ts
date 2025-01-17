export class ScopeDimensionFilterDto {
    dimensionId: number = 0;
    aggregationId: number = 0;

    constructor(id : number, lid : number) {
        this.dimensionId = id,
        this.aggregationId = lid
    }
}