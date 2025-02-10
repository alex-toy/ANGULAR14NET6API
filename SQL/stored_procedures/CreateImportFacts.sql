CREATE OR ALTER PROCEDURE CreateImportFacts
    @importFacts ImportFactType READONLY
AS
------------------------------------------------------------------------
--DECLARE @ImportFacts dbo.ImportFactType;

--INSERT INTO @ImportFacts VALUES
--(1, 111, 'sales', 'xxxxx', 'carrefour', 'home', '', '2024-M04')
------------------------------------------------------------------------
BEGIN
	DECLARE @inserted_id INT;

    INSERT INTO ExecutionLogs (ProcedureName, ParameterValues, StartTime) 
	VALUES ('CreateImportFacts', '', GETDATE());
	SET @inserted_id = SCOPE_IDENTITY();

    BEGIN TRY

		IF OBJECT_ID('tempdb..#dimensionIds') IS NOT NULL DROP TABLE #dimensionIds;
		SELECT ROW_NUMBER() OVER (ORDER BY id) AS DimensionIndex, Id INTO #dimensionIds FROM Dimensions;
		DECLARE @dimension1Id INT = (SELECT Id FROM #dimensionIds WHERE DimensionIndex = 1);
		DECLARE @dimension2Id INT = (SELECT Id FROM #dimensionIds WHERE DimensionIndex = 2);
		DECLARE @dimension3Id INT = (SELECT Id FROM #dimensionIds WHERE DimensionIndex = 3);
		DECLARE @dimension4Id INT = (SELECT Id FROM #dimensionIds WHERE DimensionIndex = 4);

		IF OBJECT_ID('tempdb..#facts') IS NOT NULL DROP TABLE #facts;
		CREATE TABLE #facts (
			RowNumber INT, Amount DECIMAL(18, 2), DataTypeId INT, 
			Dimension1AggregationId INT, Dimension2AggregationId INT, Dimension3AggregationId INT, Dimension4AggregationId INT, 
			Dimension1Id INT, Dimension2Id INT, Dimension3Id INT, Dimension4Id INT, 
			TimeAggregationId INT);
		INSERT INTO #facts (
			RowNumber, Amount, DataTypeId, 
			Dimension1AggregationId, Dimension2AggregationId, Dimension3AggregationId, Dimension4AggregationId, 
			Dimension1Id, Dimension2Id, Dimension3Id, Dimension4Id, 
			TimeAggregationId)
		SELECT
			IMPF.RowNumber, IMPF.Amount, DT.Id AS DataTypeId,
			CASE WHEN AGG1.Id IS NULL THEN -1 ELSE AGG1.Id END AS Dimension1AggregationId,
			CASE WHEN AGG2.Id IS NULL THEN -1 ELSE AGG2.Id END AS Dimension2AggregationId,
			CASE WHEN AGG3.Id IS NULL THEN -1 ELSE AGG3.Id END AS Dimension3AggregationId,
			CASE WHEN AGG4.Id IS NULL THEN -1 ELSE AGG4.Id END AS Dimension4AggregationId,
			CASE WHEN LV1.DimensionId IS NULL THEN -1 ELSE LV1.DimensionId END AS Dimension1Id,
			CASE WHEN LV2.DimensionId IS NULL THEN -1 ELSE LV2.DimensionId END AS Dimension2Id,
			CASE WHEN LV3.DimensionId IS NULL THEN -1 ELSE LV3.DimensionId END AS Dimension3Id,
			CASE WHEN LV4.DimensionId IS NULL THEN -1 ELSE LV4.DimensionId END AS Dimension4Id,
			Tim.Id AS TimeAggregationId
		FROM @importFacts IMPF
		LEFT JOIN DataTypes DT ON DT.Label = IMPF.DataType
		LEFT JOIN Aggregations AGG1 ON AGG1.Label = IMPF.Dimension1Aggregation
		LEFT JOIN Levels LV1 ON AGG1.LevelId = LV1.Id
		LEFT JOIN Aggregations AGG2 ON AGG2.Label = IMPF.Dimension2Aggregation
		LEFT JOIN Levels LV2 ON AGG2.LevelId = LV2.Id
		LEFT JOIN Aggregations AGG3 ON AGG3.Label = IMPF.Dimension3Aggregation
		LEFT JOIN Levels LV3 ON AGG3.LevelId = LV3.Id
		LEFT JOIN Aggregations AGG4 ON AGG4.Label = IMPF.Dimension4Aggregation
		LEFT JOIN Levels LV4 ON AGG4.LevelId = LV4.Id
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
		
		-- errors wrongly positionned aggregation
		INSERT INTO #errors (RowNumber, Description)
		SELECT
			IMPF.RowNumber,
			CONCAT(
				'aggregation inexistante ou mal positionnée : ',
				CAST(IMPF.Amount AS VARCHAR), ' - ', 
				IMPF.DataType, ' - ', 
				IMPF.Dimension1Aggregation, ' - ',
				IMPF.Dimension2Aggregation, ' - ',
				IMPF.Dimension3Aggregation, ' - ',
				IMPF.Dimension4Aggregation, ' - ',
				IMPF.TimeAggregation
			) AS Description
		FROM #facts TFA
		JOIN @importFacts IMPF ON IMPF.RowNumber = TFA.RowNumber
		WHERE 
			TFA.Dimension1Id != @dimension1Id OR 
			TFA.Dimension2Id != @dimension2Id OR 
			TFA.Dimension3Id != @dimension3Id OR 
			TFA.Dimension4Id != @dimension4Id OR
			TFA.DataTypeId IS NULL;

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
		LEFT JOIN #errors ERR ON ERR.RowNumber = TFA.RowNumber
		WHERE ERR.RowNumber IS NULL;
		DECLARE @LinesCreatedCount INT = (SELECT @@ROWCOUNT);

		-- return data
		SELECT RowNumber, Description FROM #errors;
		SELECT @LinesCreatedCount AS LinesCreatedCount;

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
