export class FactCreateDto {
    Amount: number;
    DataTypeId: number;
    Dimension1AggregationId: number;
    Dimension2AggregationId?: number;
    Dimension3AggregationId?: number;
    Dimension4AggregationId?: number;
    TimeAggregationId: number;

    constructor(
        Amount: number,
        DataTypeId: number,
        TimeAggregationId: number,
        Dimension1AggregationId: number,
        Dimension2AggregationId?: number,
        Dimension3AggregationId?: number,
        Dimension4AggregationId?: number,
    ) {
        this.Amount = Amount;
        this.DataTypeId = DataTypeId;
        this.Dimension1AggregationId = Dimension1AggregationId;
        this.Dimension2AggregationId = Dimension2AggregationId;
        this.Dimension3AggregationId = Dimension3AggregationId;
        this.Dimension4AggregationId = Dimension4AggregationId;
        this.TimeAggregationId = TimeAggregationId;
    }
}
