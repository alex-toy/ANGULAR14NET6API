export class FactCreateDto {
    dataTypeId: number = 0;
    amount : number = 0;
    aggregationIds: number[] = [];
    TimeAggregationId: number;

    constructor(t : number, a : number, d : number[], l : number){
        this.dataTypeId = t;
        this.amount = a;
        this.aggregationIds = d;
        this.TimeAggregationId = l;
    }
}