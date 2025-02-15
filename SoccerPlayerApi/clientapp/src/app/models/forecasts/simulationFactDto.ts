export class SimulationFactDto {
    simulationId: number;
    amount: number;
    timeAggregationId: number;

    constructor(simulationId: number, amount: number, timeAggregationId: number) {
        this.simulationId = simulationId;
        this.amount = amount;
        this.timeAggregationId = timeAggregationId;
    }
}
