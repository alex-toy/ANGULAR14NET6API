CREATE OR ALTER PROCEDURE GetTimeSeries
    @EnvironmentId INT
AS
------------------------------------------------------------------------
--declare @EnvironmentId int = 11;

--IF OBJECT_ID('tempdb..#timeSeries') IS NOT NULL DROP TABLE #timeSeries;
--CREATE TABLE #timeSeries (TimeLabel NVARCHAR(100), TimeLevelId INT, Amount DECIMAL(18, 2), DataTypeId INT, EnvironmentScopeId INT);
------------------------------------------------------------------------
BEGIN
	DECLARE @inserted_id INT;

    INSERT INTO ExecutionLogs (ProcedureName, ParameterValues, StartTime) 
	VALUES ('GetTimeSeries', CAST(@EnvironmentId AS NVARCHAR(MAX)), GETDATE());
	SET @inserted_id = SCOPE_IDENTITY();

    BEGIN TRY
	
		DECLARE @sql NVARCHAR(MAX) = N'
			INSERT INTO #timeSeries (TimeLabel, TimeLevelId, Amount, DataTypeId, EnvironmentScopeId)
			SELECT 
				T_AGG.Value AS TimeLabel,
				T_AGG.TimeLevelId AS TimeLevelId,
				MIN(FACT.Amount) AS Amount,
				MIN(FACT.DataTypeId) AS DataTypeId,
				ESCP.Id AS EnvironmentScopeId
			FROM EnvironmentScopes ESCP
			JOIN Aggregations AGG_D1 ON AGG_D1.Id = ESCP.Dimension1AggregationId
			JOIN AggregationFact AF1 ON AF1.AggregationId = AGG_D1.Id
			¤d2¤
			¤d3¤
			¤d4¤
			JOIN Facts FACT ON AF1.FactId = FACT.Id AND AF2.FactId = FACT.Id AND AF3.FactId = FACT.Id
			JOIN TimeAggregations T_AGG ON T_AGG.Id = FACT.TimeAggregationId
			WHERE 
				ESCP.EnvironmentId = @EnvironmentId
			GROUP BY T_AGG.Value, T_AGG.TimeLevelId, ESCP.Id;';

		DECLARE @dimensionCount INT = (SELECT COUNT(*) from Dimensions);

	
		IF @dimensionCount >= 2
		BEGIN
			SET @sql = REPLACE(@sql, '¤d2¤', N'JOIN Aggregations AGG_D2 ON AGG_D2.Id = ESCP.Dimension2AggregationId JOIN AggregationFact AF2 ON AF2.AggregationId = AGG_D2.Id');
		END
		ELSE
		BEGIN
			SET @sql = REPLACE(@sql, '¤d2¤', N'');
		END

		IF @dimensionCount >= 3
		BEGIN
			SET @sql = REPLACE(@sql, '¤d3¤', N'JOIN Aggregations AGG_D3 ON AGG_D3.Id = ESCP.Dimension3AggregationId JOIN AggregationFact AF3 ON AF3.AggregationId = AGG_D3.Id');
		END
		ELSE
		BEGIN
			SET @sql = REPLACE(@sql, '¤d3¤', N'');
		END

		IF @dimensionCount >= 4
		BEGIN
			SET @sql = REPLACE(@sql, '¤d4¤', N'JOIN Aggregations AGG_D4 ON AGG_D4.Id = ESCP.Dimension4AggregationId JOIN AggregationFact AF4 ON AF4.AggregationId = AGG_D4.Id');
		END
		ELSE
		BEGIN
			SET @sql = REPLACE(@sql, '¤d4¤', N'');
		END

		EXEC sp_executesql @sql, N'@EnvironmentId INT', @EnvironmentId;
	
		UPDATE ExecutionLogs
		SET EndTime = GETDATE(), Status = 'Ok'
		WHERE LogID = @inserted_id;
    END TRY

    BEGIN CATCH
		UPDATE ExecutionLogs
		SET EndTime = GETDATE(), ErrorMessage = ERROR_MESSAGE(), Status = 'Failed'
		WHERE LogID = @inserted_id;
    END CATCH
END
