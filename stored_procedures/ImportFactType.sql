CREATE TYPE dbo.ImportFactType AS TABLE
(
    Amount DECIMAL(18, 2),
    DataType NVARCHAR(50),
    Dimension1Aggregation NVARCHAR(50),
    Dimension2Aggregation NVARCHAR(50),
    Dimension3Aggregation NVARCHAR(50),
    Dimension4Aggregation NVARCHAR(50),
    TimeAggregation NVARCHAR(50)
);