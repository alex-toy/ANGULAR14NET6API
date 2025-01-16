import { ResultDto } from "../resultDto";
import { GetAggregationDto } from "./getAggregationDto";

export interface GetAggregationsResultDto extends ResultDto {
    aggregations : GetAggregationDto[]
}