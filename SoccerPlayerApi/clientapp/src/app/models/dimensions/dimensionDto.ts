import { GetLevelDto } from "../levels/getLevelDto";

export interface DimensionDto {
    id: number;
    label: string;
    levels : GetLevelDto[]
}