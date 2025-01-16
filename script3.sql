
;WITH 
	DIM_PROD AS (
		SELECT 
			FA.Id AS FactId, FA.Type AS Type, FA.Amount AS Amount,
			LV.Id AS LevelId, LV.Value,
			PROD.Id AS DimensionId,
			DV.Value AS AggregationValue
		FROM [VisionDb].[dbo].Facts FA
		JOIN [VisionDb].[dbo].DimensionFact DF ON DF.FactId = FA.Id
		JOIN [VisionDb].[dbo].DimensionValues DV ON DV.Id = DF.DimensionValueId
		JOIN [VisionDb].[dbo].Levels LV ON LV.Id = DV.LevelId
		JOIN [VisionDb].[dbo].Dimensions PROD ON PROD.Id = LV.DimensionId
		WHERE PROD.Id = 1
	),
	DIM_LOC AS (
		SELECT 
			FA.Id AS FactId, FA.Type AS Type, FA.Amount AS Amount,
			LV.Id AS LevelId, LV.Value,
			LOC.Id AS DimensionId,
			DV.Value AS AggregationValue
		FROM [VisionDb].[dbo].Facts FA
		JOIN [VisionDb].[dbo].DimensionFact DF ON DF.FactId = FA.Id
		JOIN [VisionDb].[dbo].DimensionValues DV ON DV.Id = DF.DimensionValueId
		JOIN [VisionDb].[dbo].Levels LV ON LV.Id = DV.LevelId
		JOIN [VisionDb].[dbo].Dimensions LOC ON LOC.Id = LV.DimensionId
		WHERE LOC.Id = 2
	),
	TIM_LOC AS (
		SELECT 
			FA.Id AS FactId, FA.Type AS Type, FA.Amount AS Amount,
			LV.Id AS LevelId, LV.Value,
			TIM.Id AS DimensionId,
			DV.Value AS AggregationValue
		FROM [VisionDb].[dbo].Facts FA
		JOIN [VisionDb].[dbo].DimensionFact DF ON DF.FactId = FA.Id
		JOIN [VisionDb].[dbo].DimensionValues DV ON DV.Id = DF.DimensionValueId
		JOIN [VisionDb].[dbo].Levels LV ON LV.Id = DV.LevelId
		JOIN [VisionDb].[dbo].Dimensions TIM ON TIM.Id = LV.DimensionId
		WHERE TIM.Id = 3
	)
SELECT 
	DP.FactId, DP.Type, DP.Amount,
	--DP.DimensionId AS PROD_DIM_ID, DP.LevelId AS PROD_LV_ID, DP.Value AS PROD_VAL,
	--DL.DimensionId AS LOC_DIM_ID, DL.LevelId AS LOC_LV_ID, DL.Value AS LOC_VAL,
	TIM.LevelId AS Time_Level_Id, TIM.Value AS TIM_VAL, TIM.AggregationValue
FROM DIM_PROD DP
JOIN DIM_LOC DL ON DL.FactId = DP.FactId
JOIN TIM_LOC TIM ON TIM.FactId = DP.FactId
WHERE 
	DP.LevelId = 4 AND 
	DL.LevelId = 2 -- 2, 5