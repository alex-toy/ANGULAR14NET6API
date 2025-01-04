


with 
	DIM1 as (
		SELECT 
			FA.Id, FA.Type, FA.Amount, DV.Value as value1, LV.Value as Level1, DIM.Value as dimension1
		FROM Facts FA
		join DimensionFact DF on DF.FactId = FA.Id
		join DimensionValues DV on DV.Id = DF.DimensionValueId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Product'
	),
	DIM2 as (
		SELECT 
			FA.Id, FA.Type, FA.Amount, DV.Value as value2, LV.Value as Level2, DIM.Value as dimension2
		FROM Facts FA
		join DimensionFact DF on DF.FactId = FA.Id
		join DimensionValues DV on DV.Id = DF.DimensionValueId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Location'
	),
	DIM3 as (
		SELECT 
			FA.Id, FA.Type, FA.Amount, DV.Value as value3, LV.Value as Level3, DIM.Value as dimension3
		FROM Facts FA
		join DimensionFact DF on DF.FactId = FA.Id
		join DimensionValues DV on DV.Id = DF.DimensionValueId
		join Levels LV on DV.LevelId = LV.Id
		join Dimensions DIM on DIM.Id = LV.DimensionId
		where DIM.Value = 'Time'
	)
select 
	D1.Id, D1.Amount, D1.Type, 
	D1.dimension1, D1.Level1, D1.value1, 
	D2.dimension2, D2.Level2, D2.value2,
	D3.dimension3, D3.Level3, D3.value3
from DIM1 D1
join DIM2 D2 on D1.Id = D2.Id
join DIM3 D3 on D3.Id = D2.Id


