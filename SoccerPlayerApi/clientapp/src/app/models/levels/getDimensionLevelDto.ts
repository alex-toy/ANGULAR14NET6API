import { GetLevelDto } from "./getLevelDto";

export interface GetDimensionLevelDto {
    dimensionId: number;
    value: string;
    levels: GetLevelDto[];
}