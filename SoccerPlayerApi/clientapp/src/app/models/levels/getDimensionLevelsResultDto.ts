import { ResultDto } from "../resultDto";
import { GetDimensionLevelDto } from "./getDimensionLevelDto";

export interface GetDimensionLevelsResultDto extends ResultDto {
    dimensionLevels: GetDimensionLevelDto[];
}