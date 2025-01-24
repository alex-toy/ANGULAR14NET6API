export class FactCreateDto {
    type: string = "";
    amount : number = 0;
    aggregationIds: number[] = [];
    TimeAggregationId: number;

    constructor(t : string, a : number, d : number[], l : string){
        this.type = t;
        this.amount = a;
        this.aggregationIds = d;
        this.TimeAggregationId = 0;
    }
}