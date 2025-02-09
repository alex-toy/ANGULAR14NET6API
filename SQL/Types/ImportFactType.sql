CREATE TYPE dbo.ImportFactType AS TABLE
(
    RowNumber INT,
    Amount DECIMAL(18, 2),
    DataType NVARCHAR(50),
    Dimension1Aggregation NVARCHAR(50) NOT NULL,
    Dimension2Aggregation NVARCHAR(50) DEFAULT NULL,
    Dimension3Aggregation NVARCHAR(50) DEFAULT NULL,
    Dimension4Aggregation NVARCHAR(50) DEFAULT NULL,
    TimeAggregation NVARCHAR(50)
);
