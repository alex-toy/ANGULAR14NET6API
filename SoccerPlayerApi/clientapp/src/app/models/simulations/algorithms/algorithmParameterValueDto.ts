export class AlgorithmParameterValueDto {
    simulationFactId: number;
    value: string;

    constructor(simulationFactId: number, value: string) {
        this.simulationFactId = simulationFactId;
        this.value = value;
    }
}
