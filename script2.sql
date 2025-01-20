
with 
	DIM1 as (
		SELECT
			FA.Id, 
			DV.Value as value1, 
			LV.id as LevelId1
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Product'
	),
	DIM2 as (
		SELECT 
			FA.Id, 
			DV.Value as value2, 
			LV.id as LevelId2
		FROM Facts FA
		join AggregationFact DF on DF.FactId = FA.Id
		join Aggregations DV on DV.Id = DF.AggregationId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Location'
	)
select distinct
	D1.LevelId1, D1.value1,
	D2.LevelId2, D2.value2
from DIM1 D1
join DIM2 D2 on D1.Id = D2.Id


