using System.Data;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Bos;
using SoccerPlayerApi.Controllers;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities.Environments;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Dimensions;

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

    public async Task<IEnumerable<EnvironmentScopeDto>> GetScopes(ScopeFilterDto? scopeFilter)
    {
        List<int> dimensionIds = _context.Dimensions.Select(d => d.Id).ToList();
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
                           LevelLabel = lv.Value,
                           AggregationId = agg.Id,
                           DimensionId = lv.DimensionId,
                           DimensionLabel = lv.Dimension.Value,
                           AggregationLabel = agg.Value,
                       };

            axises.Add(axis);
        }

        IQueryable<EnvironmentScopeDto> result = JoinAxes(axises, dimensionCount);

        List<EnvironmentScopeDto> distinctResult = await result.Distinct().ToListAsync();
        return distinctResult ?? new List<EnvironmentScopeDto>();
    }

    public async Task<IEnumerable<EnvironmentScopeDto>> GetScopesByEnvironmentId(int environmentId)
    {
        List<EnvironmentScope>? environmentScopes = await _context.EnvironmentScopes
                                                                        .Include(es => es.Dimension1Aggregation)
                                                                        .Include(es => es.Dimension2Aggregation)
                                                                        .Include(es => es.Dimension3Aggregation)
                                                                        .Include(es => es.Dimension4Aggregation)
                                                                        .Where(d => d.EnvironmentId == environmentId)
                                                                        .ToListAsync();

        if (environmentScopes is null) throw new Exception("environment scopes don't exist");

        IEnumerable<EnvironmentScopeDto> scopes = environmentScopes.Select(es => ExtractScopeDto(es));

        return scopes ?? new List<EnvironmentScopeDto>();
    }

    private static EnvironmentScopeDto ExtractScopeDto(EnvironmentScope es)
    {
        if (
            es.Dimension4Id is not null && es.Dimension4AggregationId is not null && es.Dimension4Aggregation is not null &&
            es.Dimension3Id is not null && es.Dimension3AggregationId is not null && es.Dimension3Aggregation is not null &&
            es.Dimension2Id is not null && es.Dimension2AggregationId is not null && es.Dimension2Aggregation is not null
        )
        {
            return new EnvironmentScopeDto
            {
                Dimension1Id = es.Dimension1Id,
                Dimension1AggregationId = es.Dimension1AggregationId,
                Level1Label = es.Dimension1Aggregation.Level.Value,
                Dimension1AggregationLabel = es.Dimension1Aggregation.Value,

                Dimension2Id = es.Dimension2Id,
                Dimension2AggregationId = es.Dimension2AggregationId,
                Level2Label = es.Dimension2Aggregation.Level.Value,
                Dimension2AggregationLabel = es.Dimension2Aggregation.Value,

                Dimension3Id = es.Dimension3Id,
                Dimension3AggregationId = es.Dimension3AggregationId,
                Level3Label = es.Dimension3Aggregation.Level.Value,
                Dimension3AggregationLabel = es.Dimension3Aggregation.Value,

                Dimension4Id = es.Dimension4Id,
                Dimension4AggregationId = es.Dimension4AggregationId,
                Level4Label = es.Dimension4Aggregation.Level.Value,
                Dimension4AggregationLabel = es.Dimension4Aggregation.Value,

                SortingValue = es.SortingValue
            };
        }

        if (
            es.Dimension3Id is not null && es.Dimension3AggregationId is not null && es.Dimension3Aggregation is not null &&
            es.Dimension2Id is not null && es.Dimension2AggregationId is not null && es.Dimension2Aggregation is not null
        )
        {
            return new EnvironmentScopeDto
            {
                Dimension1Id = es.Dimension1Id,
                Dimension1AggregationId = es.Dimension1AggregationId,
                Level1Label = es.Dimension1Aggregation.Value,
                Dimension1AggregationLabel = es.Dimension1Aggregation.Value,

                Dimension2Id = es.Dimension2Id,
                Dimension2AggregationId = es.Dimension2AggregationId,
                Level2Label = es.Dimension2Aggregation.Value,
                Dimension2AggregationLabel = es.Dimension2Aggregation.Value,

                Dimension3Id = es.Dimension3Id,
                Dimension3AggregationId = es.Dimension3AggregationId,
                Level3Label = es.Dimension3Aggregation.Value,
                Dimension3AggregationLabel = es.Dimension3Aggregation.Value,

                SortingValue = es.SortingValue
            };
        }

        if (es.Dimension2Id is not null && es.Dimension2AggregationId is not null && es.Dimension2Aggregation is not null)
        {
            return new EnvironmentScopeDto
            {
                Dimension1Id = es.Dimension1Id,
                Dimension1AggregationId = es.Dimension1AggregationId,
                Level1Label = es.Dimension1Aggregation.Value,
                Dimension1AggregationLabel = es.Dimension1Aggregation.Value,

                Dimension2Id = es.Dimension2Id,
                Dimension2AggregationId = es.Dimension2AggregationId,
                Level2Label = es.Dimension2Aggregation.Value,
                Dimension2AggregationLabel = es.Dimension2Aggregation.Value,

                SortingValue = es.SortingValue
            };
        }

        return new EnvironmentScopeDto
        {
            Dimension1Id = es.Dimension1Id,
            Dimension1AggregationId = es.Dimension1AggregationId,
            Level1Label = es.Dimension1Aggregation.Value,
            Dimension1AggregationLabel = es.Dimension1Aggregation.Value,

            SortingValue = es.SortingValue
        };
    }

    public async Task<IEnumerable<EnvironmentScopeDto>> GetScopesByEnvironmentIdOld(int environmentId)
    {
        Entities.Environment? environment = _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .FirstOrDefault(d => d.Id == environmentId);

        if (environment is null) throw new Exception("environment doesn't exist");

        int dimensionCount = 0;
        List<IQueryable<AxisDto>> axises = new List<IQueryable<AxisDto>>();

        if (environment.LevelFilter1?.DimensionId is not null)
        {
            dimensionCount += 1;
            AddAxis(axises, environment.LevelFilter1.DimensionId, environment.LevelIdFilter1);
        }

        if (environment.LevelFilter2?.DimensionId is not null && environment.LevelIdFilter2 is not null)
        {
            dimensionCount += 1;
            AddAxis(axises, environment.LevelFilter2.DimensionId, environment.LevelIdFilter2.Value);
        }

        if (environment.LevelFilter3?.DimensionId is not null && environment.LevelIdFilter3 is not null)
        {
            dimensionCount += 1;
            AddAxis(axises, environment.LevelFilter3.DimensionId, environment.LevelIdFilter3.Value);
        }

        IQueryable<EnvironmentScopeDto> result = JoinAxes(axises, dimensionCount);

        List<EnvironmentScopeDto> distinctResult = await result.Distinct().ToListAsync();
        return distinctResult ?? new List<EnvironmentScopeDto>();
    }

    public async Task<IEnumerable<GetScopeDataDto>> GetScopeData(EnvironmentScopeDto scope)
    {
        int dimensionCount = _context.Dimensions.Count();

        if (dimensionCount == 0) throw new Exception("no dimensions exist");

        IQueryable<GetScopeDataDto> resultQuery;
        if (dimensionCount == 4)
        {
            resultQuery = GetScopeDataFor_4_Dimensions(scope);
        }
        else if (dimensionCount == 3)
        {
            resultQuery = GetScopeDataFor_3_Dimensions(scope);
        }
        else if (dimensionCount == 2)
        {
            resultQuery = GetScopeDataFor_2_Dimensions(scope);
        }
        else
        {
            resultQuery = GetScopeDataFor_1_Dimensions(scope);
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

    public async Task<int> CreateFactAsync(FactCreateDto fact)
    {
        int dimensionCount = _context.Dimensions.Count();
        if (fact.AggregationIds.Count() != dimensionCount) throw new Exception("dimension count doesn't match");

        bool areDimensionsCovered = await _dimensionService.GetAreDimensionsCovered(fact, dimensionCount);
        if (!areDimensionsCovered) throw new Exception("dimensions are not all covered");

        bool isFactExists = await GetFactExists(fact);
        if (isFactExists) throw new Exception("fact already exists");

        Fact factDb = new() { Amount = fact.Amount, DataTypeId = fact.DataTypeId, TimeAggregationId = fact.TimeAggregationId };
        int entityId = await _factRepo.CreateAsync(factDb);

        AddAggregationFacts(fact, factDb, entityId);

        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<IEnumerable<ImportFactCreateResultDto>> CreateImportFactAsync(IEnumerable<ImportFactDto> facts)
    {
        List<ImportFactCreateResultDto> result = new ();

        DataTable factsTable = GetDataTable(facts);

        using var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "CreateImportFacts";

        SqlParameter factsParameter = new ()
        {
            ParameterName = "@Facts",
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.ImportFactType",
            Value = factsTable
        };

        command.Parameters.Add(factsParameter);

        await command.ExecuteNonQueryAsync();

        return result;
    }

    public async Task<IEnumerable<DataTypeDto>> GetFactTypes()
    {
        return await _context.Facts
            .Include(f => f.DataType)
            .Select(f => new DataTypeDto { Label = f.DataType.Label, Id = f.DataTypeId })
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<DataTypeDto>> GetDataTypes()
    {
        return await _context.DataTypes
            .Select(f => new DataTypeDto { Label = f.Label, Id = f.Id })
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

        return await _context.TimeAggregations.Where(a => string.Compare(a.Value, presentDate) <= 0 && a.TimeLevelId == levelId)
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

    public async Task<DataTypeDto> CreateTypeAsync(TypeCreateDto type)
    {
        DataType typeDb = new() { Label = type.Label };
        EntityEntry<DataType> entityId = await _context.DataTypes.AddAsync(typeDb);

        await _context.SaveChangesAsync();
        return new DataTypeDto { Id = entityId.Entity.Id, Label = type.Label };
    }

    private void AddAxis(List<IQueryable<AxisDto>> axises, int dimensionId, int levelId)
    {
        var axis1 = from fa in _context.Facts
                    join df in _context.AggregationFacts on fa.Id equals df.FactId
                    join agg in _context.Aggregations on df.AggregationId equals agg.Id
                    join lv in _context.Levels on agg.LevelId equals lv.Id
                    join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                    where dim.Id == dimensionId && lv.Id == levelId
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

    private static IQueryable<EnvironmentScopeDto> JoinAxes(List<IQueryable<AxisDto>> axises, int dimensionCount)
    {
        if (dimensionCount == 1) return JoinAxisFor1Dimensions(axises);

        if (dimensionCount == 2) return JoinAxisFor2Dimensions(axises);

        if (dimensionCount == 3) return JoinAxisFor3Dimensions(axises);

        return JoinAxisFor4Dimensions(axises);
    }

    private static IQueryable<EnvironmentScopeDto> JoinAxisFor4Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               join d3 in axises[2] on d1.FactId equals d3.FactId
               join d4 in axises[3] on d1.FactId equals d4.FactId
               select new EnvironmentScopeDto
               {
                   Dimension1Id = d1.DimensionId,
                   Dimension1AggregationId = d1.AggregationId,
                   Level1Label = d1.LevelLabel,
                   Dimension1Label = d1.DimensionLabel,
                   Dimension1AggregationLabel = d1.AggregationLabel,

                   Dimension2Id = d2.DimensionId,
                   Dimension2AggregationId = d2.AggregationId,
                   Level2Label = d2.LevelLabel,
                   Dimension2Label = d2.DimensionLabel,
                   Dimension2AggregationLabel = d2.AggregationLabel,

                   Dimension3Id = d3.DimensionId,
                   Dimension3AggregationId = d3.AggregationId,
                   Level3Label = d3.LevelLabel,
                   Dimension3Label = d3.DimensionLabel,
                   Dimension3AggregationLabel = d3.AggregationLabel,

                   Dimension4Id = d4.DimensionId,
                   Dimension4AggregationId = d4.AggregationId,
                   Level4Label = d4.LevelLabel,
                   Dimension4Label = d4.DimensionLabel,
                   Dimension4AggregationLabel = d4.AggregationLabel,
               };
    }

    private static IQueryable<EnvironmentScopeDto> JoinAxisFor3Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               join d3 in axises[2] on d1.FactId equals d3.FactId
               select new EnvironmentScopeDto
               {
                   Dimension1Id = d1.DimensionId,
                   Dimension1AggregationId = d1.AggregationId,
                   Level1Label = d1.LevelLabel,
                   Dimension1Label = d1.DimensionLabel,
                   Dimension1AggregationLabel = d1.AggregationLabel,

                   Dimension2Id = d2.DimensionId,
                   Dimension2AggregationId = d2.AggregationId,
                   Level2Label = d2.LevelLabel,
                   Dimension2Label = d2.DimensionLabel,
                   Dimension2AggregationLabel = d2.AggregationLabel,

                   Dimension3Id = d3.DimensionId,
                   Dimension3AggregationId = d3.AggregationId,
                   Level3Label = d3.LevelLabel,
                   Dimension3Label = d3.DimensionLabel,
                   Dimension3AggregationLabel = d3.AggregationLabel,
               };
    }

    private static IQueryable<EnvironmentScopeDto> JoinAxisFor2Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               select new EnvironmentScopeDto
               {
                   Dimension1Id = d1.DimensionId,
                   Dimension1AggregationId = d1.AggregationId,
                   Level1Label = d1.LevelLabel,
                   Dimension1Label = d1.DimensionLabel,
                   Dimension1AggregationLabel = d1.AggregationLabel,

                   Dimension2Id = d2.DimensionId,
                   Dimension2AggregationId = d2.AggregationId,
                   Level2Label = d2.LevelLabel,
                   Dimension2Label = d2.DimensionLabel,
                   Dimension2AggregationLabel = d2.AggregationLabel
               };
    }

    private static IQueryable<EnvironmentScopeDto> JoinAxisFor1Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               select new EnvironmentScopeDto
               {
                   Dimension1Id = d1.DimensionId,
                   Dimension1AggregationId = d1.AggregationId,
                   Level1Label = d1.LevelLabel,
                   Dimension1Label = d1.DimensionLabel,
                   Dimension1AggregationLabel = d1.AggregationLabel
               };
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_1_Dimensions(EnvironmentScopeDto scope)
    {
        IQueryable<AxisBo> dim1 = GetDimensionAxis(scope, 1);
        IQueryable<AxisBo> timeDimension = GetTimeDimensionAxis();

        var resultQuery = from d1 in dim1
                          join tim in timeDimension on d1.FactId equals tim.FactId
                          where
                            d1.AggId == scope.Dimension1AggregationId
                          select new GetScopeDataDto
                          {
                              FactId = d1.FactId,
                              TypeId = d1.TypeId,
                              TypeLabel = d1.TypeLabel,
                              Amount = d1.Amount,
                              AggregationIds = new() { d1.AggId },
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_2_Dimensions(EnvironmentScopeDto scope)
    {
        IQueryable<AxisBo> dim1 = GetDimensionAxis(scope, 1);
        IQueryable<AxisBo> dim2 = GetDimensionAxis(scope, 2);
        IQueryable<AxisBo> timeDimension = GetTimeDimensionAxis();

        var resultQuery = from d1 in dim1
                          join d2 in dim2 on d1.FactId equals d2.FactId
                          join tim in timeDimension on d1.FactId equals tim.FactId
                          where
                            d1.AggId == scope.Dimension1AggregationId &&
                            d2.AggId == scope.Dimension2AggregationId
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_3_Dimensions(EnvironmentScopeDto scope)
    {
        IQueryable<AxisBo> dim1 = GetDimensionAxis(scope, 1);
        IQueryable<AxisBo> dim2 = GetDimensionAxis(scope, 2);
        IQueryable<AxisBo> dim3 = GetDimensionAxis(scope, 3);
        IQueryable<AxisBo> timeDimension = GetTimeDimensionAxis();

        var resultQuery = from d1 in dim1
                          join d2 in dim2 on d1.FactId equals d2.FactId
                          join d3 in dim3 on d1.FactId equals d3.FactId
                          join tim in timeDimension on d1.FactId equals tim.FactId
                          where
                            d1.AggId == scope.Dimension1AggregationId &&
                            d2.AggId == scope.Dimension2AggregationId &&
                            d3.AggId == scope.Dimension3AggregationId
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_4_Dimensions(EnvironmentScopeDto scope)
    {
        IQueryable<AxisBo> dim1 = GetDimensionAxis(scope, 1);
        IQueryable<AxisBo> dim2 = GetDimensionAxis(scope, 2);
        IQueryable<AxisBo> dim3 = GetDimensionAxis(scope, 3);
        IQueryable<AxisBo> dim4 = GetDimensionAxis(scope, 4);
        IQueryable<AxisBo> timeDimension = GetTimeDimensionAxis();

        var resultQuery = from d1 in dim1
                          join d2 in dim2 on d1.FactId equals d2.FactId
                          join d3 in dim3 on d1.FactId equals d3.FactId
                          join d4 in dim4 on d1.FactId equals d4.FactId
                          join tim in timeDimension on d1.FactId equals tim.FactId
                          where
                            d1.AggId == scope.Dimension1AggregationId &&
                            d2.AggId == scope.Dimension2AggregationId &&
                            d3.AggId == scope.Dimension3AggregationId &&
                            d4.AggId == scope.Dimension4AggregationId
                          select new GetScopeDataDto
                          {
                              FactId = d1.FactId,
                              TypeId = d1.TypeId,
                              TypeLabel = d1.TypeLabel,
                              Amount = d1.Amount,
                              AggregationIds = new() { d1.AggId, d2.AggId, d3.AggId, d4.AggId },
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

    private IQueryable<AxisBo> GetDimensionAxis(EnvironmentScopeDto scope, int dimensionIndex)
    {
        return from fa in _context.Facts
               join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
               join df in _context.AggregationFacts on fa.Id equals df.FactId
               join dv in _context.Aggregations on df.AggregationId equals dv.Id
               join lv in _context.Levels on dv.LevelId equals lv.Id
               join prod in _context.Dimensions on lv.DimensionId equals prod.Id
               where prod.Id == (dimensionIndex == 1 
                                    ? scope.Dimension1Id 
                                    : dimensionIndex == 2 
                                        ? scope.Dimension2Id 
                                        : dimensionIndex == 3
                                            ? scope.Dimension3Id
                                            : scope.Dimension4Id)
               select new AxisBo
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
    }

    private IQueryable<AxisBo> GetTimeDimensionAxis()
    {
        return from fa in _context.Facts
               join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
               join dv in _context.TimeAggregations on fa.TimeAggregationId equals dv.Id
               join lv in _context.TimeLevels on dv.TimeLevelId equals lv.Id
               select new AxisBo
               {
                   FactId = fa.Id,
                   TypeLabel = dt.Label,
                   TypeId = dt.Id,
                   Amount = fa.Amount,
                   LevelId = lv.Id,
                   Value = lv.Value,
                   DimensionId = 0,
                   AggregationValue = dv.Value,
                   AggId = dv.Id
               };
    }

    private static DataTable GetDataTable(IEnumerable<ImportFactDto> facts)
    {
        var factsTable = new DataTable();
        factsTable.Columns.Add("Amount", typeof(decimal));
        factsTable.Columns.Add("DataType", typeof(string));
        factsTable.Columns.Add("Dimension1Aggregation", typeof(string));
        factsTable.Columns.Add("Dimension2Aggregation", typeof(string));
        factsTable.Columns.Add("Dimension3Aggregation", typeof(string));
        factsTable.Columns.Add("Dimension4Aggregation", typeof(string));
        factsTable.Columns.Add("TimeAggregation", typeof(string));

        foreach (var fact in facts)
        {
            factsTable.Rows.Add(fact.Amount, fact.DataType, fact.Dimension1Aggregation,
                                fact.Dimension2Aggregation, fact.Dimension3Aggregation,
                                fact.Dimension4Aggregation, fact.TimeAggregation);
        }

        return factsTable;
    }
}
