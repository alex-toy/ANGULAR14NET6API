CREATE OR ALTER PROCEDURE CreateImportFacts
    @facts ImportFactType READONLY
AS
------------------------------------------------------------------------
--declare @EnvironmentId int = 11;
------------------------------------------------------------------------
BEGIN
	DECLARE @inserted_id INT;

    INSERT INTO ExecutionLogs (ProcedureName, ParameterValues, StartTime) 
	VALUES ('CreateImportFacts', '', GETDATE());
	SET @inserted_id = SCOPE_IDENTITY();

    BEGIN TRY
	
		INSERT INTO Facts (Amount, DataTypeId, Dimension1AggregationId, Dimension2AggregationId, Dimension3AggregationId, Dimension4AggregationId, TimeAggregationId)
		SELECT 
			IMPF.Amount, 
			DT.Id AS DataTypeId,
			CASE WHEN AGG1.Id IS NULL THEN -1 ELSE AGG1.Id END AS Dimension1AggregationId,
			CASE WHEN AGG2.Id IS NULL THEN -1 ELSE AGG2.Id END AS Dimension2AggregationId,
			CASE WHEN AGG3.Id IS NULL THEN -1 ELSE AGG3.Id END AS Dimension3AggregationId,
			CASE WHEN AGG4.Id IS NULL THEN -1 ELSE AGG4.Id END AS Dimension4AggregationId,
			Tim.Id AS TimeAggregationId
		FROM @facts IMPF
		LEFT JOIN DataTypes DT ON DT.Label = IMPF.DataType
		LEFT JOIN Aggregations AGG1 ON AGG1.Value = IMPF.Dimension1Aggregation
		LEFT JOIN Aggregations AGG2 ON AGG2.Value = IMPF.Dimension2Aggregation
		LEFT JOIN Aggregations AGG3 ON AGG3.Value = IMPF.Dimension3Aggregation
		LEFT JOIN Aggregations AGG4 ON AGG4.Value = IMPF.Dimension4Aggregation
		LEFT JOIN TimeAggregations TIM ON TIM.Label = IMPF.TimeAggregation;
		
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
