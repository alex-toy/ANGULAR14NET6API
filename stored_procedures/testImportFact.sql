
DECLARE @ImportFacts dbo.ImportFactType;

INSERT INTO @ImportFacts (Amount, DataType, Dimension1Aggregation, Dimension2Aggregation, Dimension3Aggregation, TimeAggregation)
VALUES 
(9155, 'sales', 'auchan', 'france', 'home', '2024-M06'),
(8541, 'sales', 'auchan', 'lyon', 'sport', '2024-M03'),
(8542, 'sales', 'carrefour', 'marseille', 'alimentaire', '2024-M05')


INSERT INTO Facts (Amount, DataTypeId, Dimension1AggregationId, Dimension2AggregationId, Dimension3AggregationId, TimeAggregationId)
SELECT 
    IMPF.Amount, 
    DT.Id AS DataTypeId, 
    AGG1.Id AS Dimension1AggregationId, 
    AGG2.Id AS Dimension2AggregationId, 
    AGG3.Id AS Dimension3AggregationId, 
    Tim.Id AS TimeAggregationId
FROM @ImportFacts IMPF
LEFT JOIN DataTypes DT ON DT.Label = IMPF.DataType
LEFT JOIN Aggregations AGG1 ON AGG1.Value = IMPF.Dimension1Aggregation
LEFT JOIN Aggregations AGG2 ON AGG2.Value = IMPF.Dimension2Aggregation
LEFT JOIN Aggregations AGG3 ON AGG3.Value = IMPF.Dimension3Aggregation
LEFT JOIN TimeAggregations TIM ON TIM.Value = IMPF.TimeAggregation;
