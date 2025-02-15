import { TimeDimensionDto } from "./timeDimensionDto";

export interface GetScopeDataDto {
    factId: number;
    dataTypeId: number;
    typeLabel: string;
    amount: number;
    timeDimension: TimeDimensionDto ;
    aggregationIds : number[];
}
