using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Utils;
using System.Linq;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Services.Facts;

public class FactService : IFactService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Fact> _factRepo;
    private readonly IDimensionService _dimensionService;

    public FactService(ApplicationDbContext context, IGenericRepo<Fact> factRepo, IDimensionService dimensionService)
    {
        _context = context;
        _factRepo = factRepo;
        _dimensionService = dimensionService;
    }

    public async Task<IEnumerable<ScopeDto>> GetScopes(ScopeFilterDto? scopeFilter)
    {
        List<int> dimensionIds = _context.Dimensions.Where(d => d.Id != GlobalVar.TIME_DIMENSION_ID).Select(d => d.Id).ToList();
        int dimensionCount = dimensionIds.Count();
        List<IQueryable<AxisDto>> axises = new List<IQueryable<AxisDto>>();
        foreach (var currentDimensionId in dimensionIds)
        {
            ScopeDimensionFilterDto? correspondingFilter = scopeFilter?.ScopeDimensionFilters.FirstOrDefault(f => f.DimensionId == currentDimensionId);

            var axis = from fa in _context.Facts
                       join df in _context.AggregationFacts on fa.Id equals df.FactId
                       join agg in _context.Aggregations on df.AggregationId equals agg.Id
                       join lv in _context.Levels on agg.LevelId equals lv.Id
                       join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                       where (correspondingFilter != null)
                            ? (dim.Id == currentDimensionId && lv.Id == correspondingFilter.LevelId)
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

    public async Task<IEnumerable<ScopeDto>> GetScopesByEnvironmentId(int environmentId)
    {
        Entities.Environment? environment = _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .Include(e => e.LevelFilter5)
            .FirstOrDefault(d => d.Id == environmentId);

        if (environment is null) throw new Exception("environment doesn't exist");

        int dimensionCount = 0;
        List<IQueryable<AxisDto>> axises = new List<IQueryable<AxisDto>>();

        if (environment.LevelFilter1?.DimensionId is not null)
        {
            dimensionCount += 1;
            var axis1 = from fa in _context.Facts
                        join df in _context.AggregationFacts on fa.Id equals df.FactId
                        join agg in _context.Aggregations on df.AggregationId equals agg.Id
                        join lv in _context.Levels on agg.LevelId equals lv.Id
                        join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                        where dim.Id == environment.LevelFilter1.DimensionId && lv.Id == environment.LevelIdFilter1
                        select new AxisDto
                        {
                            FactId = fa.Id,
                            LevelLabel = agg.Value,
                            AggregationId = agg.Id,
                            DimensionId = lv.DimensionId,
                            DimensionLabel = lv.Dimension.Value,
                        };

            axises.Add(axis1);
        }

        if (environment.LevelFilter2?.DimensionId is not null)
        {
            dimensionCount += 1;
            var axis1 = from fa in _context.Facts
                        join df in _context.AggregationFacts on fa.Id equals df.FactId
                        join agg in _context.Aggregations on df.AggregationId equals agg.Id
                        join lv in _context.Levels on agg.LevelId equals lv.Id
                        join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                        where dim.Id == environment.LevelFilter2.DimensionId && lv.Id == environment.LevelIdFilter2
                        select new AxisDto
                        {
                            FactId = fa.Id,
                            LevelLabel = agg.Value,
                            AggregationId = agg.Id,
                            DimensionId = lv.DimensionId,
                            DimensionLabel = lv.Dimension.Value,
                        };

            axises.Add(axis1);
        }

        if (environment.LevelFilter3?.DimensionId is not null)
        {
            dimensionCount += 1;
            var axis1 = from fa in _context.Facts
                        join df in _context.AggregationFacts on fa.Id equals df.FactId
                        join agg in _context.Aggregations on df.AggregationId equals agg.Id
                        join lv in _context.Levels on agg.LevelId equals lv.Id
                        join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                        where dim.Id == environment.LevelFilter3.DimensionId && lv.Id == environment.LevelIdFilter3
                        select new AxisDto
                        {
                            FactId = fa.Id,
                            LevelLabel = agg.Value,
                            AggregationId = agg.Id,
                            DimensionId = lv.DimensionId,
                            DimensionLabel = lv.Dimension.Value,
                        };

            axises.Add(axis1);
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
        if (dimensionCount == 4)
        {
            resultQuery = GetScopeDataFor_4_Dimensions(scope);
        }
        else if (dimensionCount == 3)
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
            .Where(f => f.TimeAggregationId == filter.TimeAggregationId)
            .Include(f => f.AggregationFacts).ThenInclude(df => df.Aggregation).ThenInclude(dv => dv.Level).ThenInclude(l => l.Dimension)
            .Select(f => new GetFactResultDto
            {
                Id = f.Id,
                Amount = f.Amount,
                DataType = f.DataTypeId,
                Dimensions = f.AggregationFacts.Select(df => new DimensionResultDto
                {
                    DimensionValueId = df.AggregationId,
                    Value = df.Aggregation.Value,
                    DimensionId = df.Aggregation.Level.DimensionId,
                    DimensionLabel = df.Aggregation.Level.Dimension.Value,
                    LevelId = df.Aggregation.Level.Id,
                    LevelLabel = df.Aggregation.Level.Value,
                }).ToList(),
            });

        if (filter.DataTypeId is not null) facts = facts.Where(fr => fr.DataType == filter.DataTypeId);

        if (filter.FactDimensionFilters is not null && filter.FactDimensionFilters.Count > 0)
        {
            for (int index = 0; index < Math.Min(filter.FactDimensionFilters.Count(), _context.Dimensions.Count()); index++)
            {
                GetFactDimensionFilterDto factDimensionFilter = filter.FactDimensionFilters.ElementAt(index);
                facts = facts.Where(DimensionFilter(factDimensionFilter));
            }
        }

        if (filter.AggregationIds is not null && filter.AggregationIds.Count > 0)
        {
            facts = facts.Where(_dimensionService.DimensionValueFilter(filter.AggregationIds));
        }

        return await facts.ToListAsync();
    }

    public async Task<FactCreateResultDto> CreateFactAsync(FactCreateDto fact)
    {
        int dimensionCount = _context.Dimensions.Count() - 1;
        if (fact.AggregationIds.Count() != dimensionCount) return new FactCreateResultDto
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

        Fact factDb = new() { Amount = fact.Amount, DataTypeId = fact.DataTypeId, TimeAggregationId = fact.TimeAggregationId };
        int entityId = await _factRepo.CreateAsync(factDb);

        AddAggregationFacts(fact, factDb, entityId);

        await _context.SaveChangesAsync();
        return new FactCreateResultDto { IsSuccess = true, FactId = entityId };
    }

    public async Task<IEnumerable<TypeDto>> GetFactTypes()
    {
        return await _context.Facts
            .Include(f => f.DataType)
            .Select(f => new TypeDto { Label = f.DataType.Label, Id = f.DataTypeId })
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<TypeDto>> GetTypes()
    {
        return await _context.DataTypes
            .Select(f => new TypeDto { Label = f.Label, Id = f.Id })
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeAggregationDto>> GetTimeAggregations(int levelId)
    {
        string presentKey = levelId switch
        {
            4 => "PresentMonthDate",
            5 => "PresentWeekDate",
            _ => "PresentMonthDate"
        };

        string pastSpan = levelId switch
        {
            4 => "PastMonthSpan",
            5 => "PastWeekSpan",
            _ => "PastMonthSpan"
        };
        string presentDate = _context.Settings.Where(s => s.Key == presentKey).FirstOrDefault()!.Value;
        string pastSpanString = _context.Settings.Where(s => s.Key == pastSpan).FirstOrDefault()!.Value;
        int.TryParse(pastSpanString, out int span);

        return await _context.Aggregations.Where(a => string.Compare(a.Value, presentDate) <= 0 && a.LevelId == levelId)
                                               .OrderByDescending(a => a.Value)
                                               .Take(span)
                                               .Select(agg => new TimeAggregationDto
                                               {
                                                   TimeAggregationId = agg.Id,
                                                   Label = agg.Value
                                               })
                                               .ToListAsync();
    }

    public async Task<bool> UpdateFactAsync(FactUpdateDto fact)
    {
        Fact? factDb = await _factRepo.GetByIdAsync(fact.FactId);

        if (factDb is null) return false;

        factDb.Amount = fact.Amount ?? factDb.Amount;
        factDb.DataTypeId = fact.DataTypeId ?? factDb.DataTypeId;

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
            DataTypeId = fact.DataTypeId,
            AggregationIds = fact.AggregationIds.ToList(),
            TimeAggregationId = fact.TimeAggregationId
        });

        return facts.Count() > 0;
    }

    private void AddAggregationFacts(FactCreateDto fact, Fact factDb, int entityId)
    {
        IEnumerable<AggregationFact> dimensionFacts = fact.AggregationIds.Select(id => new AggregationFact
        {
            AggregationId = id,
            FactId = entityId,
        });

        factDb.AggregationFacts = dimensionFacts.ToList();

        factDb.TimeAggregationId = fact.TimeAggregationId;
    }

    private IQueryable<GetScopeDataDto> GetScopeDataFor_3_Dimensions(ScopeDto scope)
    {
        var dim1 = from fa in _context.Facts
                   join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                   join df in _context.AggregationFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.AggregationId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join prod in _context.Dimensions on lv.DimensionId equals prod.Id
                   where prod.Id == scope.Aggregations[0].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       TypeLabel = dt.Label,
                       TypeId = dt.Id,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = prod.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var dim2 = from fa in _context.Facts
                   join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                   join df in _context.AggregationFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.AggregationId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join loc in _context.Dimensions on lv.DimensionId equals loc.Id
                   where loc.Id == scope.Aggregations[1].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       TypeLabel = dt.Label,
                       TypeId = dt.Id,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = loc.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var timeDimension = from fa in _context.Facts
                            join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                            join dv in _context.Aggregations on fa.TimeAggregationId equals dv.Id
                            join lv in _context.Levels on dv.LevelId equals lv.Id
                            join tim in _context.Dimensions on lv.DimensionId equals tim.Id
                            where tim.Id == GlobalVar.TIME_DIMENSION_ID
                            select new
                            {
                                FactId = fa.Id,
                                TypeLabel = dt.Label,
                                TypeId = dt.Id,
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
                              TypeId = d1.TypeId,
                              TypeLabel = d1.TypeLabel,
                              Amount = d1.Amount,
                              AggregationIds = new() { d1.AggId, d2.AggId },
                              TimeDimension = new TimeDimensionDto
                              {
                                  TimeLevelId = tim.LevelId,
                                  TimeAggregationId = tim.AggId,
                                  TimeAggregationLabel = tim.Value,
                                  TimeAggregationValue = tim.AggregationValue
                              }
                          };

        return resultQuery;
    }

    private IQueryable<GetScopeDataDto> GetScopeDataFor_4_Dimensions(ScopeDto scope)
    {
        var dim1 = from fa in _context.Facts
                   join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                   join df in _context.AggregationFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.AggregationId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join prod in _context.Dimensions on lv.DimensionId equals prod.Id
                   where prod.Id == scope.Aggregations[0].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       TypeLabel = dt.Label,
                       TypeId = dt.Id,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = prod.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var dim2 = from fa in _context.Facts
                   join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                   join df in _context.AggregationFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.AggregationId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join loc in _context.Dimensions on lv.DimensionId equals loc.Id
                   where loc.Id == scope.Aggregations[1].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       Type = dt.Label,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = loc.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var dim3 = from fa in _context.Facts
                   join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                   join df in _context.AggregationFacts on fa.Id equals df.FactId
                   join dv in _context.Aggregations on df.AggregationId equals dv.Id
                   join lv in _context.Levels on dv.LevelId equals lv.Id
                   join loc in _context.Dimensions on lv.DimensionId equals loc.Id
                   where loc.Id == scope.Aggregations[2].DimensionId
                   select new
                   {
                       FactId = fa.Id,
                       TypeLabel = dt.Label,
                       TypeId = dt.Id,
                       Amount = fa.Amount,
                       LevelId = lv.Id,
                       Value = lv.Value,
                       DimensionId = loc.Id,
                       AggregationValue = dv.Value,
                       AggId = dv.Id
                   };

        var timeDimension = from fa in _context.Facts
                            join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
                            join dv in _context.Aggregations on fa.TimeAggregationId equals dv.Id
                            join lv in _context.Levels on dv.LevelId equals lv.Id
                            join tim in _context.Dimensions on lv.DimensionId equals tim.Id
                            where tim.Id == GlobalVar.TIME_DIMENSION_ID
                            select new
                            {
                                FactId = fa.Id,
                                Type = dt.Label,
                                Amount = fa.Amount,
                                LevelId = lv.Id,
                                Value = lv.Value,
                                DimensionId = tim.Id,
                                AggregationValue = dv.Value,
                                AggId = dv.Id
                            };

        var resultQuery = from d1 in dim1
                          join d2 in dim2 on d1.FactId equals d2.FactId
                          join d3 in dim3 on d1.FactId equals d3.FactId
                          join tim in timeDimension on d1.FactId equals tim.FactId
                          where
                            d1.AggId == scope.Aggregations[0].AggregationId &&
                            d2.AggId == scope.Aggregations[1].AggregationId &&
                            d3.AggId == scope.Aggregations[2].AggregationId
                          select new GetScopeDataDto
                          {
                              FactId = d1.FactId,
                              TypeId = d1.TypeId,
                              TypeLabel = d1.TypeLabel,
                              Amount = d1.Amount,
                              AggregationIds = new() { d1.AggId, d2.AggId, d3.AggId },
                              TimeDimension = new TimeDimensionDto
                              {
                                  TimeLevelId = tim.LevelId,
                                  TimeAggregationId = tim.AggId,
                                  TimeAggregationLabel = tim.Value,
                                  TimeAggregationValue = tim.AggregationValue
                              }
                          };

        return resultQuery;
    }

    public async Task<TypeDto> CreateTypeAsync(TypeCreateDto type)
    {
        DataType typeDb = new() { Label = type.Label };
        EntityEntry<DataType> entityId = await _context.DataTypes.AddAsync(typeDb);

        await _context.SaveChangesAsync();
        return new TypeDto { Id = entityId.Entity.Id, Label = type.Label };
    }
}
