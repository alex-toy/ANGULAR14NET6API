
DECLARE @ImportFacts dbo.ImportFactType;

INSERT INTO @ImportFacts (Amount, DataType, Dimension1Aggregation, Dimension2Aggregation, Dimension3Aggregation, Dimension4Aggregation, TimeAggregation)
VALUES 
(111, 'sales', 'auchan', 'france', 'home', NULL, '2024-M06'),
(222, 'sales', 'auchan', 'lyon', 'sport', NULL, '2024-M03'),
(333, 'sales', 'carrefour', 'marseille', 'alimentaire', NULL, '2024-M05')


exec CreateImportFacts @ImportFacts
