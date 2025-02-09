
DECLARE @ImportFacts dbo.ImportFactType;

INSERT INTO @ImportFacts (RowNumber, Amount, DataType, Dimension1Aggregation, Dimension2Aggregation, Dimension3Aggregation, Dimension4Aggregation, TimeAggregation)
VALUES 
(1, 1111111, 'sales', 'auchan', 'france', 'home', '', '2024-M06'),
(2, 2222222, 'sales', 'auchan', 'lyon', 'sport', '', '2024-M03'),
(3, 3333333, 'sales', 'carrefour', 'marseille', 'alimentaire', '', '2024-M05'),
(4, 3333333, 'sales', 'carrefour', 'marseille', 'alimentaire', '', '2023-M07')


exec CreateImportFacts @ImportFacts
