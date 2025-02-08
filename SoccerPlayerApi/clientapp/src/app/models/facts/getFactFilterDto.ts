
export class GetFactFilterDto {
    dataTypeId?: number;

    dimension1Id: number;
    dimension1AggregationId: number;
    dimension1LevelId: number;

    dimension2Id?: number;
    dimension2AggregationId?: number;
    dimension2LevelId?: number;

    dimension3Id?: number;
    dimension3AggregationId?: number;
    dimension3LevelId?: number;

    dimension4Id?: number;
    dimension4AggregationId?: number;
    dimension4LevelId?: number;

    constructor(
        dimension1Id: number,
        dimension1AggregationId: number,
        dimension1LevelId: number,
        dimension2Id: number,
        dimension2AggregationId?: number,
        dimension2LevelId?: number,
        dimension3Id?: number,
        dimension3AggregationId?: number,
        dimension3LevelId?: number,
        dimension4Id?: number,
        dimension4AggregationId?: number,
        dimension4LevelId?: number,
        dataTypeId?: number
    ) {
        this.dimension1Id = dimension1Id;
        this.dimension1AggregationId = dimension1AggregationId;
        this.dimension1LevelId = dimension1LevelId;
        this.dimension2Id = dimension2Id;
        this.dimension2AggregationId = dimension2AggregationId;
        this.dimension2LevelId = dimension2LevelId;
        this.dimension3Id = dimension3Id;
        this.dimension3AggregationId = dimension3AggregationId;
        this.dimension3LevelId = dimension3LevelId;
        this.dimension4Id = dimension4Id;
        this.dimension4AggregationId = dimension4AggregationId;
        this.dimension4LevelId = dimension4LevelId;
        this.dataTypeId = dataTypeId;
    }
}
