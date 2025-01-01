SELECT *
FROM Facts SL
join DimensionFact DF on DF.FactId = SL.Id
join DimensionValues DV on DV.Id = DF.DimensionValueId
where FactId = 9