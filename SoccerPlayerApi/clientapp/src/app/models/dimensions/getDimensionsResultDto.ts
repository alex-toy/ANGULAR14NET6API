import { ResultDto } from "../resultDto";
import { DimensionDto } from "./dimensionDto";

export interface GetDimensionsResultDto extends ResultDto {
    dimensions : DimensionDto[]
}