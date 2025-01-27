
WITH 
	DIM1 AS (
		SELECT AGG.*, ES.EnvironmentId, FACT.Amount, FACT.Id AS FactId1
		FROM EnvironmentScopes ES
		join Aggregations AGG on AGG.Id = ES.Dimension1AggregationId
		join AggregationFact FA on FA.AggregationId = AGG.Id
		join Facts FACT on FA.FactId = FACT.Id
		where ES.Id = 28
	),
	DIM2 AS (
		SELECT AGG.*, ES.EnvironmentId, FACT.Amount, FACT.Id AS FactId2
		FROM EnvironmentScopes ES
		join Aggregations AGG on AGG.Id = ES.Dimension2AggregationId
		join AggregationFact FA on FA.AggregationId = AGG.Id
		join Facts FACT on FA.FactId = FACT.Id
		where ES.Id = 28
	),
	DIM3 AS (
		SELECT AGG.*, ES.EnvironmentId, FACT.Amount, FACT.Id AS FactId3
		FROM EnvironmentScopes ES
		join Aggregations AGG on AGG.Id = ES.Dimension3AggregationId
		join AggregationFact FA on FA.AggregationId = AGG.Id
		join Facts FACT on FA.FactId = FACT.Id
		where ES.Id = 28
	)
SELECT *
FROM DIM1
JOIN DIM2 ON DIM1.FactId1 = DIM2.FactId2
JOIN DIM3 ON DIM1.FactId1 = DIM3.FactId3
where DIM1.EnvironmentId = 20






--WITH 
--DIM1 AS (
--	SELECT
--		ENV.Id AS EnvId1, ENV.LevelIdFilter1 AS LevelId1, 
--		ESC.Dimension1Id AS DimId1, ESC.Dimension1AggregationId,
--		ESC.Id AS EnvironmentScopeId1
--	FROM Environments ENV
--	JOIN EnvironmentSortings ES ON ES.EnvironmentId = ENV.Id
--	JOIN EnvironmentScopes ESC ON ESC.EnvironmentId = ENV.Id
--	JOIN Aggregations AGG ON AGG.Id = ESC.Dimension1AggregationId
--),
--DIM2 AS (
--	SELECT
--		ENV.Id AS EnvId2, ENV.LevelIdFilter2 AS LevelId2,
--		ESC.Dimension2Id AS DimId2, ESC.Dimension2AggregationId,
--		ESC.Id AS EnvironmentScopeId2
--	FROM Environments ENV
--	JOIN EnvironmentSortings ES ON ES.EnvironmentId = ENV.Id
--	JOIN EnvironmentScopes ESC ON ESC.EnvironmentId = ENV.Id
--	JOIN Aggregations AGG ON AGG.Id = ESC.Dimension2AggregationId
--)
--SELECT * 
--FROM DIM1
--Join DIM2 ON DIM2.EnvironmentScopeId2 = DIM1.EnvironmentScopeId1
----WHERE dim1.Id = 20

