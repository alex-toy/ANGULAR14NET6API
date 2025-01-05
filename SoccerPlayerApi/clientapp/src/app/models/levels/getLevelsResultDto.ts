import { ResultDto } from "../resultDto";
import { GetLevelDto } from "./getLevelDto";

export interface GetLevelsResultDto extends ResultDto {
    levels: GetLevelDto[];  
    isSuccess: boolean;
}