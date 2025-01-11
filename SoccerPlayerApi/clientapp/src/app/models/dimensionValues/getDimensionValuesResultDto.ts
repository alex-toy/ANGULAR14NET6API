import { ResultDto } from "../resultDto";
import { GetDimensionValueDto } from "./getDimensionValueDto";

export interface GetDimensionValuesResultDto extends ResultDto {
    dimensionValues : GetDimensionValueDto[]
}