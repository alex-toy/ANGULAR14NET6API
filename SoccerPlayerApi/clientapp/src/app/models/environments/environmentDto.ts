import { EnvironmentSortingDto } from "./environmentSortingDto";

export interface EnvironmentDto {
    id: number;
    name: string;
    description: string;

    dimension1Id: number;
    levelIdFilter1: number;
    levelLabel1: string;
    
    dimension2Id: number;
    levelIdFilter2: number;
    levelLabel2: string;
    
    dimension3Id: number;
    levelIdFilter3: number;
    levelLabel3: string;
    
    dimension4Id: number;
    levelIdFilter4: number;
    levelLabel4: string;

    environmentSortings : EnvironmentSortingDto[];
}
