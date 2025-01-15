using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<DimensionValue> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<DimensionValue> dimensionValueRepo)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
    }

    public async Task<IEnumerable<DimensionDto>> GetDimensions()
    {
        return await _context.Dimensions
            .Where(d => d.Id != 3) // time
            .Select(d => new DimensionDto { Id = d.Id, Value = d.Value })
            .ToListAsync();
    }

    public async Task<int> CreateDimensionAsync(Dimension dimension)
    {
        int entityId = await _dimensionRepo.CreateAsync(dimension);
        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateDimensionValueAsync(DimensionValueCreateDto level)
    {
        EntityEntry<DimensionValue> entity = await _context.DimensionValues.AddAsync(new DimensionValue { 
            LevelId = level.LevelId,
            Value = level.Value,
        });
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<IEnumerable<GetDimensionValueDto>> GetDimensionValues(int levelId)
    {
        return await _context.DimensionValues
            .Where(dv => dv.LevelId == levelId)
            .Select(dv => new GetDimensionValueDto { LevelId = dv.Id, Value = dv.Value })
            .ToListAsync();
    }

    public async Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount)
    {
        List<int> distinctDimensionValueIds = await _context.DimensionValues
            .Where(x => fact.DimensionValueIds.Contains(x.Id))
            .Include(x => x.Level).ThenInclude(x => x.Dimension)
            .Select(x => x.Level.Dimension.Id)
            .Distinct()
            .ToListAsync();
        return distinctDimensionValueIds.Count() == dimensionCount;
    }

    private static void AddDimensionFacts(FactCreateDto fact, Fact factDb, int entityId)
    {
        IEnumerable<DimensionFact> dimensionFacts = fact.DimensionValueIds.Select(id => new DimensionFact
        {
            DimensionValueId = id,
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
                                        .All(dimensionValueId => dimensionValueIds.Contains(dimensionValueId)) &&
                             factResult.Dimensions.Count() == dimensionValueIds.Count;
    }
}
