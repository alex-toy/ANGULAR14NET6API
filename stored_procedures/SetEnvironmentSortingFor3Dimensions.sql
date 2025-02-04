CREATE OR ALTER PROCEDURE SetEnvironmentSortingFor3Dimensions
    @EnvironmentId INT
AS
BEGIN
    IF OBJECT_ID('tempdb..#environmentSortingIds') IS NOT NULL DROP TABLE #environmentSortingIds;
	SELECT 
		ESRT.Id AS EnvironmentSortingId, TimeLevelId, T_AGG_S.Value AS StartTimeLabel, T_AGG_E.Value AS EndTimeLabel, 
		ESRT.Aggregator, ESRT.DataTypeId, ESRT.OrderIndex, ESRT.IsAscending
	INTO #environmentSortingIds
	FROM environmentSortings ESRT
	JOIN Aggregations T_AGG_S ON T_AGG_S.Id = ESRT.StartTimeSpan
	JOIN Aggregations T_AGG_E ON T_AGG_E.Id = ESRT.EndTimeSpan 
	WHERE environmentId = @EnvironmentId;

	IF OBJECT_ID('tempdb..#timeSeries') IS NOT NULL DROP TABLE #timeSeries;
	SELECT 
		T_AGG.Value AS TimeLabel,
		T_AGG.LevelId AS TimeLevelId,
		MIN(FACT.Amount) AS Amount,
		MIN(FACT.DataTypeId) AS DataTypeId,
		ESCP.Id AS EnvironmentScopeId
	INTO #timeSeries
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
		ESCP.EnvironmentId = @EnvironmentId
	GROUP BY T_AGG.Value, T_AGG.LevelId, ESCP.Id

	IF OBJECT_ID('tempdb..#aggregatedValues') IS NOT NULL DROP TABLE #aggregatedValues;
	SELECT 
		EnvironmentScopeId, EnvironmentSortingId,
		CASE MIN(ESIDS.Aggregator)
			WHEN 0 THEN CASE MIN(ESIDS.IsAscending) WHEN 0 THEN -SUM(TS.Amount) WHEN 1 THEN SUM(TS.Amount) END
			WHEN 1 THEN CASE MIN(ESIDS.IsAscending) WHEN 0 THEN -AVG(TS.Amount) WHEN 1 THEN AVG(TS.Amount) END
		END AS Amount,
		MIN(ESIDS.OrderIndex) AS OrderIndex,
		MIN(ESIDS.IsAscending) AS IsAscending
	INTO #aggregatedValues
	FROM #timeSeries TS 
	CROSS JOIN #environmentSortingIds ESIDS
	WHERE 
		TS.TimeLevelId = ESIDS.TimeLevelId AND
		TS.DataTypeId = ESIDS.DataTypeId AND
		TS.TimeLabel >= ESIDS.StartTimeLabel AND
		TS.TimeLabel <= ESIDS.EndTimeLabel
	GROUP BY EnvironmentScopeId, EnvironmentSortingId;

	IF OBJECT_ID('tempdb..#sortingValues') IS NOT NULL DROP TABLE #sortingValues;
	SELECT 
		EnvironmentScopeId,
		STRING_AGG(CAST(AV.Amount AS VARCHAR), ';') WITHIN GROUP (ORDER BY OrderIndex ASC) AS ConcatenatedAmounts
	INTO #sortingValues
	FROM #aggregatedValues AV
	GROUP BY EnvironmentScopeId
	ORDER BY EnvironmentScopeId;

	UPDATE ESCP
	SET SortingValue = SV.ConcatenatedAmounts
	FROM EnvironmentScopes ESCP
	JOIN #sortingValues SV ON SV.EnvironmentScopeId = ESCP.Id
END;
