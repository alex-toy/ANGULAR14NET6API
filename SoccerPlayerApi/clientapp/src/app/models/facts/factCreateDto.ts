export class FactCreateDto {
    type: string = "";
    amount : number = 0;
    aggregationIds: number[] = [];

    constructor(){
    }
}