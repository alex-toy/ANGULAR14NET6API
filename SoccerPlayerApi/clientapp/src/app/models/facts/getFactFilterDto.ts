import { GetFactDimensionFilterDto } from "./getFactDimensionFilterDto";

export class GetFactFilterDto {
    type: string = "";
    factDimensionFilters: GetFactDimensionFilterDto[] = [];
    dimensionValueIds: number[] = [];

    constructor(){
    }
}