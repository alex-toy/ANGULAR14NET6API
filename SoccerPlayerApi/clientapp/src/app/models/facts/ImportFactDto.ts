export interface ImportFactDto {
    RowNumber : number,
    Amount: number;
    DataType: string;
    Dimension1Aggregation: string;
    Dimension2Aggregation: string | null;
    Dimension3Aggregation: string | null;
    Dimension4Aggregation: string | null;
    TimeAggregation: string;
} 
