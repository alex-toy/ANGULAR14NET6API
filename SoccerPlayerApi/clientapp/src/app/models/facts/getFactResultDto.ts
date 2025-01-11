import { DimensionResultDto } from "./dimensionResultDto";

export interface GetFactResultDto {
    id: number;
    type: string;
    amount: number;
    dimensions: DimensionResultDto[];
}