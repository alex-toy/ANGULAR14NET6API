import { TimeDimensionDto } from "./timeDimensionDto";

export interface GetScopeDataDto {
    factId: number;
    type: string;
    amount: number;
    timeDimension: TimeDimensionDto ;
}
