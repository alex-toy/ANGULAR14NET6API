import { DimensionValueDto } from "./dimensionValueDto";

export class ScopeDto {
    dimensionValues: DimensionValueDto[] = [];
    display: string = "";

    constructor(scope : ScopeDto){
        this.dimensionValues = scope.dimensionValues;
        this.display = scope.dimensionValues.map(dimensionValue => dimensionValue.value).join(' - ') ?? "";
    }
}