import { AlgorithmParameterKeyDto } from "./AlgorithmParameterKeyDto";

export class AlgorithmDto {
    id : number;
    label: string;
    keys: AlgorithmParameterKeyDto[];

    constructor(id: number, label: string, keys: AlgorithmParameterKeyDto[]) {
        this.id = id;
        this.label = label;
        this.keys = keys;
    }
}
