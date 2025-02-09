
DECLARE @ImportFacts dbo.ImportFactType;

INSERT INTO @ImportFacts (RowNumber, Amount, DataType, Dimension1Aggregation, Dimension2Aggregation, Dimension3Aggregation, Dimension4Aggregation, TimeAggregation)
VALUES 
(3, 111, 'sales', 'auchan', 'france', 'home', '', '2024-M04')


exec CreateImportFacts @ImportFacts
