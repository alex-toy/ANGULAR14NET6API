using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Repo;
using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Facts;
using System.Linq.Expressions;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;

namespace SoccerPlayerApi.Services.Facts;

public class FactService : IFactService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<DimensionValue> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;
    private readonly IDimensionService _dimensionService;

    public FactService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<DimensionValue> dimensionValueRepo, IDimensionService dimensionService)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
        _dimensionService = dimensionService;
    }

    public async Task<IEnumerable<ScopeDto>> GetScopes()
    {
        IQueryable<ScopeDto> scopes = _context.Facts
            .Include(f => f.DimensionFacts)
                .ThenInclude(df => df.DimensionValue)
                .ThenInclude(dv => dv.Level)
                .ThenInclude(l => l.Dimension)
            .Select(f => new ScopeDto
            {
                DimensionValues = f.DimensionFacts
                    .Where(df => df.DimensionValue.Level.Dimension.Value.ToLower() != "time")
                    .Select(df => new DimensionValueDto
                    {
                        Value = df.DimensionValue.Value,
                        LevelId = df.DimensionValue.Level.Id,
                        LevelLabel = df.DimensionValue.Level.Value,
                        Dimension = df.DimensionValue.Level.Dimension.Value,
                        DimensionId = df.DimensionValue.Level.Dimension.Id,
                    }).ToList(),
            });

        return await scopes.ToListAsync();
    }

    public async Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactFilterDto filter)
    {
        IQueryable<GetFactResultDto> facts = _context.Facts
            .Include(f => f.DimensionFacts).ThenInclude(df => df.DimensionValue).ThenInclude(dv => dv.Level).ThenInclude(l => l.Dimension)
            .Select(f => new GetFactResultDto
            {
                Id = f.Id,
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
            facts = facts.Where(_dimensionService.DimensionValueFilter(filter.DimensionValueIds));
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

        bool areDimensionsCovered = await _dimensionService.GetAreDimensionsCovered(fact, dimensionCount);
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

    private static Expression<Func<GetFactResultDto, bool>> DimensionFilter(GetFactDimensionFilterDto factDimensionFilter1)
    {
        return factResult => factDimensionFilter1.LevelId == factResult.Dimensions.First(x => x.DimensionId == factDimensionFilter1.DimensionId).LevelId &&
                             factDimensionFilter1.DimensionValue == factResult.Dimensions.First(x => x.DimensionId == factDimensionFilter1.DimensionId).Value;
    }

    private async Task<bool> GetFactExists(FactCreateDto fact)
    {
        IEnumerable<GetFactResultDto> facts = await GetFacts(new GetFactFilterDto
        {
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

    public async Task<IEnumerable<string>> GetFactTypes()
    {
        return await _context.Facts.Select(f => f.Type).Distinct().ToListAsync();
    }

    public async Task<bool> UpdateFactAsync(FactUpdateDto fact)
    {
        Fact? factDb = await _factRepo.GetByIdAsync(fact.Id);

        if (factDb is null) return false;

        factDb.Amount = fact.Amount;
        factDb.Type = fact.Type;

        await _context.SaveChangesAsync();

        return true;
    }
}
