export interface EnvironmentUpdateDto {
    id: number;
    name: string;
    description: string;
    levelIdFilter1: number;
    levelIdFilter2?: number;
    levelIdFilter3?: number;
    levelIdFilter4?: number;
    levelIdFilter5?: number;
}
