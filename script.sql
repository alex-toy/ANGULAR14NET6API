
with 
	DIM1 as (
		SELECT 
			FA.Id, FA.Type, FA.Amount, DV.Id as DVId1, DV.Value as value1, 
			LV.id as LevelId1, LV.Value as Level1, 
			DIM.Value as dimension1, DIM.Id as dimId1
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Product'
	),
	DIM2 as (
		SELECT 
			FA.Id, FA.Type, FA.Amount, DV.Id as DVId2, DV.Value as value2, 
			LV.id as LevelId2, LV.Value as Level2, 
			DIM.Value as dimension2, DIM.Id as dimId2
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Location'
	),
	DIM3 as (
		SELECT 
			FA.Id, FA.Type, FA.Amount, DV.Id as DVId3, DV.Value as value3, 
			LV.id as LevelId3, LV.Value as Level3, 
			DIM.Value as dimension3, DIM.Id as dimId3
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Time'
	)
select 
	D1.Amount, D1.Type, 
	D1.dimId1, D1.dimension1, D1.LevelId1, D1.Level1, D1.DVId1, D1.value1, 
	D2.dimId2, D2.dimension2, D2.LevelId2, D2.Level2, D2.DVId2, D2.value2,
	D3.dimId3, D3.dimension3, D3.LevelId3, D3.Level3, D3.DVId3, D3.value3
from DIM1 D1
join DIM2 D2 on D1.Id = D2.Id
join DIM3 D3 on D3.Id = D2.Id


