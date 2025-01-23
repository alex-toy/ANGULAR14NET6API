using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Utils;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<Aggregation> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<Aggregation> dimensionValueRepo)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
    }

    public async Task<IEnumerable<DimensionDto>> GetDimensions()
    {
        return await _context.Dimensions
            .Include(d => d.Levels)
            .Where(d => d.Id != GlobalVar.TIME_DIMENSION) // time
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
        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateDimensionValueAsync(AggregationCreateDto level)
    {
        EntityEntry<Aggregation> entity = await _context.Aggregations.AddAsync(new Aggregation { 
            LevelId = level.LevelId,
            Value = level.Value,
        });
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
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

        factDb.DimensionFacts = dimensionFacts.ToList();
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
