import { GetFactResultDto } from "./getFactResultDto";

export interface GetFactsResultDto {
    isSuccess: boolean;
    message: string;
    facts: GetFactResultDto[];
}