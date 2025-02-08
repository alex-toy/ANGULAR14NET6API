export interface CreateLevelDto {
    label: string;
    dimensionId: number;
    ancestorId : number | null;
}