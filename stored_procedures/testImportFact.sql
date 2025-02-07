

DECLARE @ImportFacts dbo.ImportFactType;

INSERT INTO @ImportFacts (Amount, DataType, Dimension1Aggregation, Dimension2Aggregation, Dimension3Aggregation, TimeAggregation)
VALUES 
(9155, 'sales', 'france', 'home', 'auchan', '2024-M06'),
(8541, 'sales', 'lyon', 'sport', 'carrefour', '2024-M03'),
(8541, 'sales', 'lyon', 'rideaux', 'carrefour 1', '2024-M05')


select IMPF.Amount, DT.Id AS DataTypeId, AGG1.Id, AGG2.Id, AGG3.Id, Tim.Id
from @ImportFacts IMPF
left join DataTypes DT ON DT.Label = IMPF.DataType
left join Aggregations AGG1 ON AGG1.Value = IMPF.Dimension1Aggregation
left join Aggregations AGG2 ON AGG2.Value = IMPF.Dimension2Aggregation
left join Aggregations AGG3 ON AGG3.Value = IMPF.Dimension3Aggregation
left join TimeAggregations TIM ON TIM.Value = IMPF.TimeAggregation



