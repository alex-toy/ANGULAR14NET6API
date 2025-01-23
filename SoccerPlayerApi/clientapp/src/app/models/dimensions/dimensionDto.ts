import { GetLevelDto } from "../levels/getLevelDto";

export interface DimensionDto {
    id: number;
    value: string;
    levels : GetLevelDto[]
}