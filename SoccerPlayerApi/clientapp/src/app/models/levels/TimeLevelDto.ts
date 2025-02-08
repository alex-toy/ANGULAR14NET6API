export class TimeLevelDto {
    id: number;
    label: string;

    ancestorId?: number;
    ancestor?: TimeLevelDto;

    constructor(id: number, label: string, ancestorId?: number, ancestor?: TimeLevelDto) {
        this.id = id;
        this.label = label;
        this.ancestorId = ancestorId;
        this.ancestor = ancestor;
    }
}
