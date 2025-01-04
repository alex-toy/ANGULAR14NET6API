using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    private object factResult;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<DimensionValue> dimensionValueRepo)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
    }

    public async Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactDto filter)
    {
        IQueryable<GetFactResultDto> temp1 = _context.Facts
            .Include(f => f.DimensionFacts).ThenInclude(df => df.DimensionValue).ThenInclude(dv => dv.Level).ThenInclude(l => l.Dimension)
            .Select(f => new GetFactResultDto
            {
                Amount = f.Amount,
                Type = f.Type,
                Dimensions = f.DimensionFacts.Select(df => new DimensionResultDto
                {
                    Value = df.DimensionValue.Value,
                    DimensionId = df.DimensionValue.Level.DimensionId,
                    DimensionLabel = df.DimensionValue.Level.Dimension.Value,
                    LevelId = df.DimensionValue.Level.Id,
                    LevelLabel = df.DimensionValue.Level.Value,
                }).ToList(),
            });

        temp1 = temp1.Where(fr => fr.Type == filter.Type);

        for(int index = 0; index < Math.Min(filter.FactDimensionFilters.Count(), _context.Dimensions.Count()); index++)
        {
            GetFactDimensionFilterDto factDimensionFilter = filter.FactDimensionFilters.ElementAt(index);
            temp1 = temp1.Where(DimensionFilter(factDimensionFilter));
        }

        return await temp1.ToListAsync();
    }

    private static Expression<Func<GetFactResultDto, bool>> DimensionFilter(GetFactDimensionFilterDto factDimensionFilter1)
    {
        return factResult => factDimensionFilter1.LevelId == factResult.Dimensions.First().LevelId &&
                             factDimensionFilter1.DimensionId == factResult.Dimensions.First().DimensionId &&
                             factDimensionFilter1.DimensionValue == factResult.Dimensions.First().Value;
    }

    public async Task<FactCreateResultDto> CreateFactAsync(FactCreateDto fact)
    {
        Fact factDb = new() { Amount = fact.Amount, Type = fact.Type };
        int entityId = await _factRepo.CreateAsync(factDb);

        int dimensionCount = _context.Dimensions.Count();
        if (fact.DimensionFacts.Count() != dimensionCount) return new FactCreateResultDto
        {
            IsSuccess = false,
            Message = "dimension count doesn't match"
        };

        bool areDimensionsCovered = await GetAreDimensionsCovered(fact, dimensionCount);
        if (!areDimensionsCovered) return new FactCreateResultDto
        {
            IsSuccess = false,
            Message = "dimensions are not all covered"
        };

        bool isFactExists = await GetFactExists(fact);
        if (isFactExists) return new FactCreateResultDto
        {
            IsSuccess = false,
            Message = "fact already exists"
        };

        AddDimensionFacts(fact, factDb, entityId);

        await _context.SaveChangesAsync();
        return new FactCreateResultDto { IsSuccess = true, FactId = entityId };
    }

    public async Task<int> CreateDimensionAsync(Dimension dimension)
    {
        int entityId = await _dimensionRepo.CreateAsync(dimension);
        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateLevelAsync(Level level)
    {
        EntityEntry<Level> entity = await _context.Levels.AddAsync(level);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<int> CreateDimensionValueAsync(DimensionValue level)
    {
        EntityEntry<DimensionValue> entity = await _context.DimensionValues.AddAsync(level);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    private async Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount)
    {
        IEnumerable<int> dimensionValueIds = fact.DimensionFacts.Select(df => df.DimensionValueId);
        List<int> distinctDimensionValueIds = await _context.DimensionValues
            .Where(x => dimensionValueIds.Contains(x.Id))
            .Include(x => x.Level).ThenInclude(x => x.Dimension)
            .Select(x => x.Level.Dimension.Id)
            .Distinct()
            .ToListAsync();
        return distinctDimensionValueIds.Count() == dimensionCount;
    }

    private async Task<bool> GetFactExists(FactCreateDto fact)
    {
        var facts = await GetFacts(new GetFactDto { Type = fact.Type });

        //return facts.Count() > 0;
        return true;
    }

    private static void AddDimensionFacts(FactCreateDto fact, Fact factDb, int entityId)
    {
        IEnumerable<DimensionFact> dimensionFacts = fact.DimensionFacts.Select(x => new DimensionFact
        {
            DimensionValueId = x.DimensionValueId,
            FactId = entityId,
        });

        factDb.DimensionFacts = dimensionFacts.ToList();
    }
}
