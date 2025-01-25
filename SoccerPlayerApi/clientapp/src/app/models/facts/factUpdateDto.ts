export class FactUpdateDto {
    factId: number = 0;
    dataTypeId: number = 0;
    amount : number = 0;

    constructor(id: number, dataTypeId: number, amount: number){
        this.factId = id;
        this.dataTypeId = dataTypeId;
        this.amount = amount;
    }
}