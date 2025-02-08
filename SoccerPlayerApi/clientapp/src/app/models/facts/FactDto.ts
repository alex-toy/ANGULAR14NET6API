import { AggregationDto } from "../aggregations/aggregationDto";
import { DataTypeDto } from "./DataTypeDto";
import { TimeAggregationDto } from "./timeAggregationDto";

export class FactDto {
    id: number;

    dataTypeId: number;
    dataType: DataTypeDto;

    amount: number;

    aggregation1: AggregationDto;

    aggregation2?: AggregationDto;

    aggregation3?: AggregationDto;

    aggregation4?: AggregationDto;

    timeAggregationId: number;
    timeAggregation: TimeAggregationDto;

    constructor(
        id: number,
        dataTypeId: number,
        dataType: DataTypeDto,
        amount: number,
        timeAggregationId: number,
        timeAggregation: TimeAggregationDto,
        aggregation1: AggregationDto,
        aggregation2?: AggregationDto,
        aggregation3?: AggregationDto,
        aggregation4?: AggregationDto
    ) {
        this.id = id;
        this.dataTypeId = dataTypeId;
        this.dataType = dataType;
        this.amount = amount;
        this.aggregation1 = aggregation1;
        this.aggregation2 = aggregation2;
        this.aggregation3 = aggregation3;
        this.aggregation4 = aggregation4;
        this.timeAggregationId = timeAggregationId;
        this.timeAggregation = timeAggregation;
    }
}
