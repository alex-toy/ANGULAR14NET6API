import { Model } from "./model";

export class Player extends Model {
    name : string;
    jerseyNumber : string;

    constructor(playerId : number, name : string, number : string){
        super(playerId);
        this.name = name;
        this.jerseyNumber = number;
    }
}
