CREATE OR ALTER PROCEDURE SetEnvironmentSorting
    @EnvironmentId INT
AS
------------------------------------------------------------------------
--declare @EnvironmentId int = 11;
------------------------------------------------------------------------
BEGIN
	DECLARE @inserted_id INT;

    INSERT INTO ExecutionLogs (ProcedureName, ParameterValues, StartTime) 
	VALUES ('SetEnvironmentSorting', CAST(@EnvironmentId AS NVARCHAR(MAX)), GETDATE());
	SET @inserted_id = SCOPE_IDENTITY();

    BEGIN TRY
		IF OBJECT_ID('tempdb..#environmentSortingIds') IS NOT NULL DROP TABLE #environmentSortingIds;
		SELECT 
			ESRT.Id AS EnvironmentSortingId, ESRT.TimeLevelId, T_AGG_S.Label AS StartTimeLabel, T_AGG_E.Label AS EndTimeLabel, 
			ESRT.Aggregator, ESRT.DataTypeId, ESRT.OrderIndex, ESRT.IsAscending
		INTO #environmentSortingIds
		FROM environmentSortings ESRT
		JOIN TimeAggregations T_AGG_S ON T_AGG_S.Id = ESRT.StartTimeSpan
		JOIN TimeAggregations T_AGG_E ON T_AGG_E.Id = ESRT.EndTimeSpan 
		WHERE environmentId = @EnvironmentId;
		
		IF OBJECT_ID('tempdb..#timeSeries') IS NOT NULL DROP TABLE #timeSeries;
		CREATE TABLE #timeSeries (TimeLabel NVARCHAR(100), TimeLevelId INT, Amount DECIMAL(18, 2), DataTypeId INT, EnvironmentScopeId INT);
		EXEC GetTimeSeries @EnvironmentId;

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
		JOIN #sortingValues SV ON SV.EnvironmentScopeId = ESCP.Id;
	
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
