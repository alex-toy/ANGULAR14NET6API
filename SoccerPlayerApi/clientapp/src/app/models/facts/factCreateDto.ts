export class FactCreateDto {
    type: string = "";
    amount : number = 0;
    dimensionValueIds: number[] = [];

    constructor(){
    }
}