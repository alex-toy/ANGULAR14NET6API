export class ScopeDimensionFilterDto {
    dimensionId: number = 0;
    levelId: number = 0;

    constructor(id : number, lid : number) {
        this.dimensionId = id,
        this.levelId = lid
    }
}