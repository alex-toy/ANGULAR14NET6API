export class FactUpdateDto {
    id: number = 0;
    type: string = "";
    amount : number = 0;

    constructor(id: number, type: string, amount: number){
        this.id = id;
        this.type = type;
        this.amount = amount;
    }
}