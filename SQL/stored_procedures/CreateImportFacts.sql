CREATE OR ALTER PROCEDURE CreateImportFacts
    @importFacts ImportFactType READONLY
AS
------------------------------------------------------------------------
--DECLARE @ImportFacts dbo.ImportFactType;

--INSERT INTO @ImportFacts VALUES 
--(1, 1111111, 'sales', 'auchan', 'france', 'home', '', '2024-M06'),
--(2, 2222222, 'sales', 'auchan', 'lyon', 'sport', '', '2024-M03'),
--(3, 3333333, 'sales', 'carrefour', 'marseille', 'alimentaire', '', '2024-M05'),
--(4, 4444444, 'sales', 'carrefour', 'marseille', 'alimentaire', '', '2023-M07')
------------------------------------------------------------------------
BEGIN
	DECLARE @inserted_id INT;

    INSERT INTO ExecutionLogs (ProcedureName, ParameterValues, StartTime) 
	VALUES ('CreateImportFacts', '', GETDATE());
	SET @inserted_id = SCOPE_IDENTITY();

    BEGIN TRY

		IF OBJECT_ID('tempdb..#facts') IS NOT NULL DROP TABLE #facts;
		CREATE TABLE #facts (RowNumber INT, Amount DECIMAL(18, 2), DataTypeId INT, Dimension1AggregationId INT, Dimension2AggregationId INT, Dimension3AggregationId INT, Dimension4AggregationId INT, TimeAggregationId INT);
		INSERT INTO #facts (RowNumber, Amount, DataTypeId, Dimension1AggregationId, Dimension2AggregationId, Dimension3AggregationId, Dimension4AggregationId, TimeAggregationId)
		SELECT 
			IMPF.RowNumber, IMPF.Amount, DT.Id AS DataTypeId,
			CASE WHEN AGG1.Id IS NULL THEN -1 ELSE AGG1.Id END AS Dimension1AggregationId,
			CASE WHEN AGG2.Id IS NULL THEN -1 ELSE AGG2.Id END AS Dimension2AggregationId,
			CASE WHEN AGG3.Id IS NULL THEN -1 ELSE AGG3.Id END AS Dimension3AggregationId,
			CASE WHEN AGG4.Id IS NULL THEN -1 ELSE AGG4.Id END AS Dimension4AggregationId,
			Tim.Id AS TimeAggregationId
		FROM @importFacts IMPF
		LEFT JOIN DataTypes DT ON DT.Label = IMPF.DataType
		LEFT JOIN Aggregations AGG1 ON AGG1.Label = IMPF.Dimension1Aggregation
		LEFT JOIN Aggregations AGG2 ON AGG2.Label = IMPF.Dimension2Aggregation
		LEFT JOIN Aggregations AGG3 ON AGG3.Label = IMPF.Dimension3Aggregation
		LEFT JOIN Aggregations AGG4 ON AGG4.Label = IMPF.Dimension4Aggregation
		LEFT JOIN TimeAggregations TIM ON TIM.Label = IMPF.TimeAggregation;

		IF OBJECT_ID('tempdb..#errors') IS NOT NULL DROP TABLE #errors;
		CREATE TABLE #errors (RowNumber INT, Description NVARCHAR(MAX));

		-- errors existing fact
		INSERT INTO #errors (RowNumber, Description)
		SELECT 
			IMPF.RowNumber,
			CONCAT(
				'existing fact : ',
				CAST(IMPF.Amount AS VARCHAR), ' - ', 
				IMPF.DataType, ' - ', 
				IMPF.Dimension1Aggregation, ' - ',
				IMPF.Dimension2Aggregation, ' - ',
				IMPF.Dimension3Aggregation, ' - ',
				IMPF.Dimension4Aggregation, ' - ',
				IMPF.TimeAggregation
			) AS Description
		FROM #facts TFA
		JOIN facts FA ON 
			FA.DataTypeId = TFA.DataTypeId AND 
			FA.Dimension1AggregationId = TFA.Dimension1AggregationId AND 
			FA.Dimension2AggregationId = TFA.Dimension2AggregationId AND 
			FA.Dimension3AggregationId = TFA.Dimension3AggregationId AND 
			FA.Dimension4AggregationId = TFA.Dimension4AggregationId AND
			FA.TimeAggregationId = TFA.TimeAggregationId
		JOIN @importFacts IMPF ON IMPF.RowNumber = TFA.RowNumber
		WHERE FA.Id IS NOT NULL;
		
		SELECT RowNumber, Description FROM #errors 

		-- ok facts
		INSERT INTO Facts (Amount, DataTypeId, Dimension1AggregationId, Dimension2AggregationId, Dimension3AggregationId, Dimension4AggregationId, TimeAggregationId)
		SELECT 
			TFA.Amount, 
			TFA.DataTypeId,
			CASE WHEN TFA.Dimension1AggregationId IS NULL THEN -1 ELSE TFA.Dimension1AggregationId END AS Dimension1AggregationId,
			CASE WHEN TFA.Dimension2AggregationId IS NULL THEN -1 ELSE TFA.Dimension2AggregationId END AS Dimension2AggregationId,
			CASE WHEN TFA.Dimension3AggregationId IS NULL THEN -1 ELSE TFA.Dimension3AggregationId END AS Dimension3AggregationId,
			CASE WHEN TFA.Dimension4AggregationId IS NULL THEN -1 ELSE TFA.Dimension4AggregationId END AS Dimension4AggregationId,
			TFA.TimeAggregationId
		FROM #facts TFA
		JOIN #errors ERR ON ERR.RowNumber = TFA.RowNumber
		WHERE ERR.RowNumber IS NULL;
		
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
