import { DimensionResultDto } from "./dimensionResultDto";

export interface GetFactResultDto {
    type: string;
    amount: number;
    dimensions: DimensionResultDto[];
}