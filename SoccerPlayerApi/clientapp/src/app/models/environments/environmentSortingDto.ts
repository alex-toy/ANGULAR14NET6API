export interface EnvironmentSortingDto {
    environmentId: number;
    aggregator: number;  // 0 for SUM, 1 for AVERAGE, 3 for Alphabetical
    startTimeSpan: number;
    endTimeSpan: number;
    isAscending: number;
    timeSpanBase: number;  // 0 for history, 1 for simulation
    dataTypeId: number;
}
  