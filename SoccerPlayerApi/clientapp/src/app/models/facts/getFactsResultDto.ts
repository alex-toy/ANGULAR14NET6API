import { ResultDto } from "../resultDto";
import { GetFactResultDto } from "./getFactResultDto";

export interface GetFactsResultDto extends ResultDto{
    facts: GetFactResultDto[];
} 