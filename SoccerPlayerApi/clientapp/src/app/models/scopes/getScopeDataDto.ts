import { TimeDimensionDto } from "./timeDimensionDto";

export interface GetScopeDataDto {
    factId: number;
    typeId: number;
    typeLabel: string;
    amount: number;
    timeDimension: TimeDimensionDto ;
    aggregationIds : number[];
}
