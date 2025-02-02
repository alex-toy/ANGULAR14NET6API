﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Levels;
using SoccerPlayerApi.Utils;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<Aggregation> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;
    private readonly ILevelService _levelService;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<Aggregation> dimensionValueRepo, ILevelService levelService)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
        _levelService = levelService;
    }

    public async Task<IEnumerable<DimensionDto>> GetDimensions()
    {
        return await _context.Dimensions
            .Include(d => d.Levels)
            .Where(d => d.Id != GlobalVar.TIME_DIMENSION_ID) // time
            .Select(d => new DimensionDto { Id = d.Id, Value = d.Value, Levels = d.Levels.Select(x => x.ToDto()).ToList() })
            .ToListAsync();
    }

    public async Task<int> GetDimensionCount()
    {
        return await _context.Dimensions.CountAsync();
    }

    public async Task<int> CreateDimensionAsync(DimensionDto dimension)
    {
        int entityId = await _dimensionRepo.CreateAsync(dimension.ToDb());

        await _levelService.CreateLevelAsync(new CreateLevelDto { Value = "all", DimensionId = entityId, AncestorId = null });

        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<IEnumerable<GetAggregationDto>> GetDimensionValues(int levelId)
    {
        return await _context.Aggregations
            .Where(dv => dv.LevelId == levelId)
            .Select(dv => new GetAggregationDto { LevelId = dv.Id, Value = dv.Value })
            .ToListAsync();
    }

    public async Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount)
    {
        List<int> distinctDimensionValueIds = await _context.Aggregations
            .Where(x => fact.AggregationIds.Contains(x.Id))
            .Include(x => x.Level).ThenInclude(x => x.Dimension)
            .Where(x => x.Level.DimensionId != GlobalVar.TIME_DIMENSION_ID)
            .Select(x => x.Level.Dimension.Id)
            .Distinct()
            .ToListAsync();
        return distinctDimensionValueIds.Count() == dimensionCount;
    }

    private static void AddDimensionFacts(FactCreateDto fact, Fact factDb, int entityId)
    {
        IEnumerable<AggregationFact> dimensionFacts = fact.AggregationIds.Select(id => new AggregationFact
        {
            AggregationId = id,
            FactId = entityId,
        });

        factDb.AggregationFacts = dimensionFacts.ToList();
    }

    private static Expression<Func<GetFactResultDto, bool>> DimensionFilter(GetFactDimensionFilterDto factDimensionFilter1)
    {
        return factResult => factDimensionFilter1.LevelId == factResult.Dimensions.First(x => x.DimensionId == factDimensionFilter1.DimensionId).LevelId &&
                             factDimensionFilter1.DimensionValue == factResult.Dimensions.First(x => x.DimensionId == factDimensionFilter1.DimensionId).Value;
    }

    public Expression<Func<GetFactResultDto, bool>> DimensionValueFilter(List<int> dimensionValueIds)
    {
        return factResult => factResult.Dimensions
                                        .Select(x => x.DimensionValueId)
                                        .All(dimensionValueId => dimensionValueIds.Contains(dimensionValueId)) && factResult.Dimensions.Count() == dimensionValueIds.Count;
    }
}
