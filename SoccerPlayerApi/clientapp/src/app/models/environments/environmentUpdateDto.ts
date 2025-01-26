import { EnvironmentSortingDto } from "./environmentSortingDto";

export interface EnvironmentUpdateDto {
    id: number;
    name: string;
    description: string;

    dimension1Id : number;
    levelIdFilter1: number;

    dimension2Id : number | null;
    levelIdFilter2: number | null;

    dimension3Id : number | null;
    levelIdFilter3: number | null;

    dimension4Id : number | null;
    levelIdFilter4: number | null;
        
    environmentSortings : EnvironmentSortingDto[];
}
