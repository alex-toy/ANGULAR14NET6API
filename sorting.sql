
WITH TEMP AS (
	SELECT 
		T_AGG.Id, T_AGG.Value, FACT.Amount
	FROM EnvironmentScopes ES
	JOIN Aggregations AGG on AGG.Id = ES.Dimension1AggregationId
	JOIN AggregationFact FA on FA.AggregationId = AGG.Id
	JOIN Facts FACT on FA.FactId = FACT.Id
	JOIN Aggregations T_AGG on T_AGG.Id = FACT.TimeAggregationId
	WHERE 
		T_AGG.LevelId = 4 AND 
		DataTypeId = 1 AND 
		ES.Id = 27 AND
		T_AGG.Value > '2022-M05' AND
		T_AGG.Value < '2024-M04'
)
SELECT SUM(Amount)
FROM TEMP