import { AlgorithmParameterValueDto } from "./algorithms/algorithmParameterValueDto";

export class FrameSimulationCreateDto {
    frameId: number;
    algorithmId: number;
    values: AlgorithmParameterValueDto[];

    constructor(
        frameId: number,
        algorithmId: number,
        values: AlgorithmParameterValueDto[]
    ) {
        this.frameId = frameId;
        this.algorithmId = algorithmId;
        this.values = values;
    }
}
