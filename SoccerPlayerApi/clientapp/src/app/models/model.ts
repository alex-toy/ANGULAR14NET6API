export class Model {
    id : number;

    constructor(playerId : number){
        this.id = playerId;
    }

    toJson(){
        return JSON.stringify(this);
    }
}