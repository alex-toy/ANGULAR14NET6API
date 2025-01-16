import { AggregationDto } from "../aggregations/aggregationDto";

export class ScopeDto {
    levelIds: string = "";
    aggregations: AggregationDto[] = [];
    display: string = "";

    constructor(scope : ScopeDto){
        this.aggregations = scope.aggregations;
        this.display = scope.aggregations.map(dimensionValue => dimensionValue.levelLabel).join(' - ') ?? "";
    }
}