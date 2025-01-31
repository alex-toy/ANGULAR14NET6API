CREATE OR ALTER PROCEDURE SetEnvironmentSortingFor3Dimensions
    @EnvironmentSortingId INT
AS
BEGIN

	DECLARE @startDate VARCHAR(8) = (SELECT AGG.Value 
									 FROM EnvironmentSortings EST 
									 JOIN Aggregations AGG ON AGG.Id = EST.StartTimeSpan 
									 WHERE EST.Id = @EnvironmentSortingId);

	DECLARE @EndDate VARCHAR(8) = (SELECT AGG.Value 
								   FROM EnvironmentSortings EST 
								   JOIN Aggregations AGG ON AGG.Id = EST.EndTimeSpan 
								   WHERE EST.Id = @EnvironmentSortingId);

	DECLARE @EnvironmentId INT = (SELECT EST.EnvironmentId FROM EnvironmentSortings EST WHERE EST.Id = @EnvironmentSortingId);

    SELECT 
		MIN(ESCP.EnvironmentId) AS EnvironmentId,
        T_AGG.Value AS TimeLabel,
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
        ESCP.EnvironmentId = @EnvironmentId AND
        T_AGG.Value >= @StartDate AND
        T_AGG.Value <= @EndDate
    GROUP BY T_AGG.Value;
END;
