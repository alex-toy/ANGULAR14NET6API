import { AlgorithmParameterValueDto } from "./algorithms/algorithmParameterValueDto";
import { SimulationFactDto } from "./simulationFactDto";

export class SimulationCreateDto {
    environmentScopeId: number;
    algorithmId: number;
    values: AlgorithmParameterValueDto[];
    simulationFacts: SimulationFactDto[];

    constructor(
        environmentScopeId: number,
        algorithmId: number,
        values: AlgorithmParameterValueDto[],
        simulationFacts: SimulationFactDto[]
    ) {
        this.environmentScopeId = environmentScopeId;
        this.algorithmId = algorithmId;
        this.values = values;
        this.simulationFacts = simulationFacts;
    }
}
