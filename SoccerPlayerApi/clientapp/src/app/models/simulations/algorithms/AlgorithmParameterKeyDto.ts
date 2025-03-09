export class AlgorithmParameterKeyDto {
    algorithmParameterKeyId: number;
    algorithmId: number;
    value: string;

    constructor(algorithmId: number, value: string, algorithmParameterKeyId: number) {
        this.algorithmParameterKeyId = algorithmParameterKeyId;
        this.algorithmId = algorithmId;
        this.value = value;
    }
}
