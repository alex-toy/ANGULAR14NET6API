
with 
	DIM1 as (
		SELECT 
			FA.Id, DT.Label, FA.Amount, DV.Id as AggrId1, DV.Value as value1, 
			LV.id as LevelId1, LV.Value as Level1, 
			DIM.Value as dimension1, DIM.Id as dimId1
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		join DataTypes DT on DT.Id = FA.DataTypeId
		where DIM.Id = 2
	),
	DIM2 as (
		SELECT 
			FA.Id, DT.Label, FA.Amount, DV.Id as AggrId2, DV.Value as value2, 
			LV.id as LevelId2, LV.Value as Level2, 
			DIM.Value as dimension2, DIM.Id as dimId2
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		join DataTypes DT on DT.Id = FA.DataTypeId
		where DIM.Id = 3
	),
	DIM3 as (
		SELECT 
			FA.Id, DT.Label, FA.Amount, DV.Id as AggrId3, DV.Value as value3, 
			LV.id as LevelId3, LV.Value as Level3, 
			DIM.Value as dimension3, DIM.Id as dimId3
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		join DataTypes DT on DT.Id = FA.DataTypeId
		where DIM.Id = 4
	),
	DIM_TIME as (
		SELECT DISTINCT
			FA.Id, DT.Label, FA.Amount, DV.Id as Tim_AggrId, DV.Value as Time_value, 
			LV.id as Time_LevelId, LV.Value as Time_Level, 
			DIM.Value as Time_dimension, DIM.Id as Time_dimId
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = FA.TimeAggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		join DataTypes DT on DT.Id = FA.DataTypeId
		where DIM.Id = 1
	)
select 
	D1.Amount, D1.Label, 
	D1.dimId1, D1.dimension1, D1.LevelId1, D1.Level1, D1.AggrId1, D1.value1, 
	D2.dimId2, D2.dimension2, D2.LevelId2, D2.Level2, D2.AggrId2, D2.value2,
	D3.dimId3, D3.dimension3, D3.LevelId3, D3.Level3, D3.AggrId3, D3.value3,
	TIM.Time_dimId, TIM.Time_dimension, TIM.Time_LevelId, TIM.Time_Level, TIM.Tim_AggrId, TIM.Time_value
from DIM1 D1
join DIM2 D2 on D1.Id = D2.Id
join DIM3 D3 on D3.Id = D2.Id
join DIM_TIME TIM on TIM.Id = D2.Id

--where D1.AggrId1 in (1, 9, 3) and D2.AggrId2 in (1, 9, 3) and D3.AggrId3 in (1, 9, 3)


