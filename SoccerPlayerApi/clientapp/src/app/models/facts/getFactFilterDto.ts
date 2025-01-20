import { GetFactDimensionFilterDto } from "./getFactDimensionFilterDto";

export class GetFactFilterDto {
    type: string = "";
    factDimensionFilters: GetFactDimensionFilterDto[] = [];
    aggregationIds: number[] = [];

    constructor(){
    }
}