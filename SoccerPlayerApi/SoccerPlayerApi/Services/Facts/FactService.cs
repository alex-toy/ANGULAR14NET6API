using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Dimensions;
using System;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Services.Facts;

public class FactService : IFactService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<Aggregation> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;
    private readonly IDimensionService _dimensionService;

    public FactService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<Aggregation> dimensionValueRepo, IDimensionService dimensionService)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
        _dimensionService = dimensionService;
    }

    public async Task<IEnumerable<ScopeDto>> GetScopes(ScopeFilterDto? scopeFilter)
    {
        List<int> dimensionIds = _context.Dimensions.Select(d => d.Id).ToList();
        int dimensionCount = dimensionIds.Count() - 1;
        List<IQueryable<AxisDto>> axises = new List<IQueryable<AxisDto>>();
        foreach (var currentDimensionId in dimensionIds)
        {
            ScopeDimensionFilterDto? correspondingFilter = scopeFilter?.ScopeDimensionFilters.FirstOrDefault(f => f.DimensionId == currentDimensionId);

            var axis = from fa in _context.Facts
                       join df in _context.DimensionFacts on fa.Id equals df.FactId
                       join agg in _context.Aggregations on df.DimensionValueId equals agg.Id
                       join lv in _context.Levels on agg.LevelId equals lv.Id
                       join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                       where (correspondingFilter != null) 
                            ? (dim.Id == currentDimensionId && lv.Id == correspondingFilter.AggregationId) 
                            : (dim.Id == currentDimensionId)
                       select new AxisDto
                       {
                           FactId = fa.Id,
                           LevelLabel = agg.Value,
                           AggregationId = agg.Id,
                           DimensionId = lv.DimensionId,
                           DimensionLabel = lv.Dimension.Value,
                       };

            axises.Add(axis);
        }

        IQueryable<ScopeDto> result = JoinAxes(axises, dimensionCount);

        List<ScopeDto> distinctResult = await result.Distinct().ToListAsync();
        return distinctResult ?? new List<ScopeDto>();
    }

    private static IQueryable<ScopeDto> JoinAxes(List<IQueryable<AxisDto>> axises, int dimensionCount)
    {
        if (dimensionCount == 2)
        {
            return from d1 in axises[0]
                   join d2 in axises[1] on d1.FactId equals d2.FactId
                   select new ScopeDto
                   {
                       Aggregations = new List<AggregationDto>()
                       {
                           new AggregationDto() { AggregationId = d1.AggregationId, LevelLabel = d1.LevelLabel, DimensionId = d1.DimensionId, Dimension = d1.DimensionLabel },
                           new AggregationDto() { AggregationId = d2.AggregationId, LevelLabel = d2.LevelLabel, DimensionId = d2.DimensionId, Dimension = d2.DimensionLabel },
                       }
                   };
        }

        if (dimensionCount == 3)
        {
            return from d1 in axises[0]
                   join d2 in axises[1] on d1.FactId equals d2.FactId
                   join d3 in axises[2] on d1.FactId equals d3.FactId
                   select new ScopeDto
                   {
                       Aggregations = new List<AggregationDto>()
                       {
                           new AggregationDto() { AggregationId = d1.AggregationId, LevelLabel = d1.LevelLabel, DimensionId = d1.DimensionId, Dimension = d1.DimensionLabel },
                           new AggregationDto() { AggregationId = d2.AggregationId, LevelLabel = d2.LevelLabel, DimensionId = d2.DimensionId, Dimension = d2.DimensionLabel },
                           new AggregationDto() { AggregationId = d3.AggregationId, LevelLabel = d3.LevelLabel, DimensionId = d3.DimensionId, Dimension = d3.DimensionLabel },
                       }
                   };
        }

        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               select new ScopeDto
               {
                   Aggregations = new List<AggregationDto>()
                   {
                       new AggregationDto() { AggregationId = d1.AggregationId, LevelLabel = d1.LevelLabel, DimensionId = d1.DimensionId, Dimension = d1.DimensionLabel },
                       new AggregationDto() { AggregationId = d2.AggregationId, LevelLabel = d2.LevelLabel, DimensionId = d2.DimensionId, Dimension = d2.DimensionLabel },
                   }
               };
    }

    public async Task<IEnumerable<GetScopeDataDto>> GetScopeData(ScopeDto scope)
    {
        int dimensionCount = _context.Dimensions.Count();
        IQueryable<GetScopeDataDto> resultQuery;
        if (dimensionCount == 3)
        {
            resultQuery = GetScopeDataFor_3_Dimensions(scope);
        }
        else
        {
            resultQuery = GetScopeDataFor_3_Dimensions(scope);
        }

        return await resultQuery.ToListAsync();
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_3_Dimensions(ScopeDto scope)
    {
        var dim1 = from fa in _context.Facts
                   join df in _context.DimensionFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.DimensionValueId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join prod in _context.Dimensions on lv.DimensionId equals prod.Id
                   where prod.Id == scope.Aggregations[0].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       Type = fa.Type,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = prod.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var dim2 = from fa in _context.Facts
                   join df in _context.DimensionFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.DimensionValueId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join loc in _context.Dimensions on lv.DimensionId equals loc.Id
                   where loc.Id == scope.Aggregations[1].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       Type = fa.Type,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = loc.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var timeDimension = from fa in _context.Facts
                            join df in _context.DimensionFacts on fa.Id equals df.FactId
                            join dv in _context.Aggregations on df.DimensionValueId equals dv.Id //DimensionValueId -> AggregationId
                            join lv in _context.Levels on dv.LevelId equals lv.Id
                            join tim in _context.Dimensions on lv.DimensionId equals tim.Id
                            where tim.Id == 3
                            select new
                            {
                                FactId = fa.Id,
                                Type = fa.Type,
                                Amount = fa.Amount,
                                LevelId = lv.Id,
                                Value = lv.Value,
                                DimensionId = tim.Id,
                                AggregationValue = dv.Value,
                                AggId = dv.Id
                            };

        var resultQuery = from d1 in dim1
                          join d2 in dim2 on d1.FactId equals d2.FactId
                          join tim in timeDimension on d1.FactId equals tim.FactId
                          where 
                            d1.AggId == scope.Aggregations[0].AggregationId && 
                            d2.AggId == scope.Aggregations[1].AggregationId
                          select new GetScopeDataDto
                          {
                              FactId = d1.FactId,
                              Type = d1.Type,
                              Amount = d1.Amount,
                              TimeDimension = new TimeDimensionDto
                              {
                                  TimeLevelId = tim.LevelId,
                                  TimeAggregationLabel = tim.Value,
                                  TimeAggregationValue = tim.AggregationValue
                              }
                          };

        return resultQuery;
    }
}
