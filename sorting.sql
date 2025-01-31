
WITH AggregatedAmounts AS (
    SELECT 
        MIN(ESCP.Id) AS EnvironmentScopeId,
        MIN(FACT.Amount) AS Amount
	FROM EnvironmentScopes ESCP
	JOIN Aggregations AGG_D1 ON AGG_D1.Id = ESCP.Dimension1AggregationId
	JOIN AggregationFact AF1 ON AF1.AggregationId = AGG_D1.Id
	JOIN Aggregations AGG_D2 ON AGG_D2.Id = ESCP.Dimension2AggregationId
	JOIN AggregationFact AF2 ON AF2.AggregationId = AGG_D2.Id
	JOIN Aggregations AGG_D3 ON AGG_D3.Id = ESCP.Dimension3AggregationId
	JOIN AggregationFact AF3 ON AF3.AggregationId = AGG_D3.Id
	JOIN Facts FACT ON AF1.FactId = FACT.Id AND AF2.FactId = FACT.Id AND AF3.FactId = FACT.Id
	JOIN Aggregations T_AGG ON T_AGG.Id = FACT.TimeAggregationId
	WHERE 
		T_AGG.LevelId = 4 AND 
		FACT.DataTypeId = 1 AND 
		ESCP.Id = 70 AND
		T_AGG.Value >= '2023-M09' AND
		T_AGG.Value <= '2024-M02'
	GROUP BY T_AGG.Value
)
SELECT SUM(Amount)
FROM EnvironmentScopes ESCP
JOIN AggregatedAmounts AM ON AM.EnvironmentScopeId = ESCP.Id
