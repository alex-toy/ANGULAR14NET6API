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
            .Select(d => new DimensionDto { Id = d.Id, Value = d.Value })
            .ToListAsync();
    }

    public async Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactFilterDto filter)
    {
        IQueryable<GetFactResultDto> facts = _context.Facts
            .Include(f => f.DimensionFacts).ThenInclude(df => df.DimensionValue).ThenInclude(dv => dv.Level).ThenInclude(l => l.Dimension)
            .Select(f => new GetFactResultDto
            {
                Amount = f.Amount,
                Type = f.Type,
                Dimensions = f.DimensionFacts.Select(df => new DimensionResultDto
                {
                    DimensionValueId = df.DimensionValueId,
                    Value = df.DimensionValue.Value,
                    DimensionId = df.DimensionValue.Level.DimensionId,
                    DimensionLabel = df.DimensionValue.Level.Dimension.Value,
                    LevelId = df.DimensionValue.Level.Id,
                    LevelLabel = df.DimensionValue.Level.Value,
                }).ToList(),
            });

        var temp = facts.ToList();

        if (!string.IsNullOrEmpty(filter.Type)) facts = facts.Where(fr => fr.Type == filter.Type);

        if (filter.FactDimensionFilters is not null && filter.FactDimensionFilters.Count > 0)
        {
            for (int index = 0; index < Math.Min(filter.FactDimensionFilters.Count(), _context.Dimensions.Count()); index++)
            {
                GetFactDimensionFilterDto factDimensionFilter = filter.FactDimensionFilters.ElementAt(index);
                facts = facts.Where(DimensionFilter(factDimensionFilter));
            }
        }

        if (filter.DimensionValueIds is not null && filter.DimensionValueIds.Count > 0)
        {
            facts = facts.Where(DimensionValueFilter(filter.DimensionValueIds));
        }

        return await facts.ToListAsync();
    }

    public async Task<FactCreateResultDto> CreateFactAsync(FactCreateDto fact)
    {
        Fact factDb = new() { Amount = fact.Amount, Type = fact.Type };
        int entityId = await _factRepo.CreateAsync(factDb);

        int dimensionCount = _context.Dimensions.Count();
        if (fact.DimensionValueIds.Count() != dimensionCount) return new FactCreateResultDto
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

    private async Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount)
    {
        List<int> distinctDimensionValueIds = await _context.DimensionValues
            .Where(x => fact.DimensionValueIds.Contains(x.Id))
            .Include(x => x.Level).ThenInclude(x => x.Dimension)
            .Select(x => x.Level.Dimension.Id)
            .Distinct()
            .ToListAsync();
        return distinctDimensionValueIds.Count() == dimensionCount;
    }

    private async Task<bool> GetFactExists(FactCreateDto fact)
    {
        IEnumerable<GetFactResultDto> facts = await GetFacts(new GetFactFilterDto { 
            Type = fact.Type,
            DimensionValueIds = fact.DimensionValueIds.ToList()
        });

        return facts.Count() > 0;
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

    private static Expression<Func<GetFactResultDto, bool>> DimensionValueFilter(List<int> dimensionValueIds)
    {
        return factResult => factResult.Dimensions
                                        .Select(x => x.DimensionValueId)
                                        .All(dimensionValueId => dimensionValueIds.Contains(dimensionValueId)) &&
                             factResult.Dimensions.Count() == dimensionValueIds.Count;
    }
}
