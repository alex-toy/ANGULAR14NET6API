export class AlgorithmDto {
    id : number;
    label: string;
    keys: string[];

    constructor(id: number, label: string, keys: string[]) {
        this.id = id;
        this.label = label;
        this.keys = keys;
    }
}
