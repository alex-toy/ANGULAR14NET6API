export class FactUpdateDto {
    factId: number = 0;
    type: string = "";
    amount : number = 0;

    constructor(id: number, type: string, amount: number){
        this.factId = id;
        this.type = type;
        this.amount = amount;
    }
}