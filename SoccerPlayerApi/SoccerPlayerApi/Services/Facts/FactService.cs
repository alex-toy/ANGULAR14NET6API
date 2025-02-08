using System.Data;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Bos;
using SoccerPlayerApi.Controllers;
using SoccerPlayerApi.Dtos.Aggregations;
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

    public async Task<IEnumerable<EnvironmentScopeDto>> GetScopes(ScopeFilterDto scopeFilter)
    {
        List<int> dimensionIds = _context.Dimensions.Select(d => d.Id).ToList();
        int dimensionCount = dimensionIds.Count();
        List<IQueryable<AxisDto>> axises = new List<IQueryable<AxisDto>>();

        AddDimension1Axis(scopeFilter.Dimension1Id, scopeFilter.Level1Id, axises);
        AddDimension2Axis(scopeFilter.Dimension2Id, scopeFilter.Level2Id, axises);
        AddDimension3Axis(scopeFilter.Dimension3Id, scopeFilter.Level3Id, axises);
        AddDimension4Axis(scopeFilter.Dimension4Id, scopeFilter.Level4Id, axises);

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

    public async Task<IEnumerable<FactDto>> GetFacts(GetFactFilterDto filter)
    {
        IQueryable<FactDto> facts = _context.Facts
            .Where(f => f.TimeAggregationId == filter.TimeAggregationId)
            .Include(f => f.Aggregation1)
            .Include(f => f.Aggregation2)
            .Include(f => f.Aggregation3)
            .Include(f => f.Aggregation4)
            .Select(fact => new FactDto
            {
                Id = fact.Id,
                Amount = fact.Amount,
                DataTypeId = fact.DataTypeId,
                Aggregation1 = new AggregationDto
                {
                    Id = fact.Aggregation1.Id,
                    Label = fact.Aggregation1.Value,
                    LevelId = fact.Aggregation1.Level.Id,
                    LevelLabel = fact.Aggregation1.Level.Value,
                    DimensionId = fact.Aggregation1.Level.DimensionId,
                    DimensionLabel = fact.Aggregation1.Level.Dimension.Label,
                }
            });

        if (filter.DataTypeId is not null) facts = facts.Where(fact => fact.DataTypeId == filter.DataTypeId);

        facts = facts.Where(filter.FactIsAtLevels());

        facts = facts.Where(filter.FactIsInAggregations());

        return await facts.ToListAsync();
    }

    public async Task<int> CreateFactAsync(FactCreateDto fact)
    {
        int dimensionCount = _context.Dimensions.Count();
        if (fact.GetDimensionCount() != dimensionCount) throw new Exception("dimension count doesn't match");

        bool areDimensionsCovered = await _dimensionService.GetAreDimensionsCovered(fact, dimensionCount);
        if (!areDimensionsCovered) throw new Exception("dimensions are not all covered");

        bool isFactExists = await GetFactExists(fact);
        if (isFactExists) throw new Exception("fact already exists");

        Fact factDb = new() { 
            Amount = fact.Amount, 
            DataTypeId = fact.DataTypeId,
            Dimension1AggregationId = fact.Dimension1AggregationId,
            Dimension2AggregationId = fact.Dimension2AggregationId,
            Dimension3AggregationId = fact.Dimension3AggregationId,
            Dimension4AggregationId = fact.Dimension4AggregationId,
            TimeAggregationId = fact.TimeAggregationId 
        };
        int entityId = await _factRepo.CreateAsync(factDb);

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
            ParameterName = "@facts",
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

    public async Task<DataTypeDto> CreateTypeAsync(DataTypeCreateDto type)
    {
        DataType typeDb = new() { Label = type.Label };
        EntityEntry<DataType> entityId = await _context.DataTypes.AddAsync(typeDb);

        await _context.SaveChangesAsync();
        return new DataTypeDto { Id = entityId.Entity.Id, Label = type.Label };
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

    private async Task<bool> GetFactExists(FactCreateDto fact)
    {
        List<Fact> facts = await _context.Facts.Where(f => 
            f.DataTypeId == fact.DataTypeId &&
            f.Dimension1AggregationId == fact.Dimension1AggregationId &&
            f.Dimension2AggregationId == fact.Dimension2AggregationId &&
            f.Dimension3AggregationId == fact.Dimension3AggregationId &&
            f.Dimension4AggregationId == fact.Dimension4AggregationId &&
            f.TimeAggregationId == fact.TimeAggregationId
        ).ToListAsync();

        return facts.Count() > 0;
    }

    private IQueryable<GetScopeDataDto> GetScopeDataFor_1_Dimensions(EnvironmentScopeDto scope)
    {
        IQueryable<AxisBo> dim1 = GetDimension1Axis(scope);
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
        IQueryable<AxisBo> dim1 = GetDimension1Axis(scope);
        IQueryable<AxisBo> dim2 = GetDimension2Axis(scope);
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
        IQueryable<AxisBo> dim1 = GetDimension1Axis(scope);
        IQueryable<AxisBo> dim2 = GetDimension2Axis(scope);
        IQueryable<AxisBo> dim3 = GetDimension3Axis(scope);
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
        IQueryable<AxisBo> dim1 = GetDimension1Axis(scope);
        IQueryable<AxisBo> dim2 = GetDimension2Axis(scope);
        IQueryable<AxisBo> dim3 = GetDimension3Axis(scope);
        IQueryable<AxisBo> dim4 = GetDimension4Axis(scope);
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

    private IQueryable<AxisBo> GetDimension1Axis(EnvironmentScopeDto scope)
    {
        return from fa in _context.Facts
               join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
               join agg in _context.Aggregations on fa.Dimension1AggregationId equals agg.Id
               join lv in _context.Levels on agg.LevelId equals lv.Id
               join prod in _context.Dimensions on lv.DimensionId equals prod.Id
               where prod.Id == scope.Dimension1Id
               select new AxisBo
               {
                   FactId = fa.Id,
                   TypeLabel = dt.Label,
                   TypeId = dt.Id,
                   Amount = fa.Amount,
                   LevelId = lv.Id,
                   Value = lv.Value,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Value,
                   AggId = agg.Id
               };
    }

    private IQueryable<AxisBo> GetDimension2Axis(EnvironmentScopeDto scope)
    {
        return from fa in _context.Facts
               join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
               join agg in _context.Aggregations on fa.Dimension2AggregationId equals agg.Id
               join lv in _context.Levels on agg.LevelId equals lv.Id
               join prod in _context.Dimensions on lv.DimensionId equals prod.Id
               where prod.Id == scope.Dimension2Id
               select new AxisBo
               {
                   FactId = fa.Id,
                   TypeLabel = dt.Label,
                   TypeId = dt.Id,
                   Amount = fa.Amount,
                   LevelId = lv.Id,
                   Value = lv.Value,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Value,
                   AggId = agg.Id
               };
    }

    private IQueryable<AxisBo> GetDimension3Axis(EnvironmentScopeDto scope)
    {
        return from fa in _context.Facts
               join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
               join agg in _context.Aggregations on fa.Dimension3AggregationId equals agg.Id
               join lv in _context.Levels on agg.LevelId equals lv.Id
               join prod in _context.Dimensions on lv.DimensionId equals prod.Id
               where prod.Id == scope.Dimension3Id
               select new AxisBo
               {
                   FactId = fa.Id,
                   TypeLabel = dt.Label,
                   TypeId = dt.Id,
                   Amount = fa.Amount,
                   LevelId = lv.Id,
                   Value = lv.Value,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Value,
                   AggId = agg.Id
               };
    }

    private IQueryable<AxisBo> GetDimension4Axis(EnvironmentScopeDto scope)
    {
        return from fa in _context.Facts
               join dt in _context.DataTypes on fa.DataTypeId equals dt.Id
               join agg in _context.Aggregations on fa.Dimension4AggregationId equals agg.Id
               join lv in _context.Levels on agg.LevelId equals lv.Id
               join prod in _context.Dimensions on lv.DimensionId equals prod.Id
               where prod.Id == scope.Dimension4Id
               select new AxisBo
               {
                   FactId = fa.Id,
                   TypeLabel = dt.Label,
                   TypeId = dt.Id,
                   Amount = fa.Amount,
                   LevelId = lv.Id,
                   Value = lv.Value,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Value,
                   AggId = agg.Id
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
                   Value = lv.Label,
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

    private void AddDimension1Axis(int? dimensionId, int levelId, List<IQueryable<AxisDto>> axises)
    {
        var axis = from fa in _context.Facts
                   join agg in _context.Aggregations on fa.Dimension1AggregationId equals agg.Id
                   join lv in _context.Levels on agg.LevelId equals lv.Id
                   join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                   where (dimensionId != null)
                        ? (dim.Id == dimensionId && lv.Id == levelId)
                        : (dim.Id == dimensionId)
                   select new AxisDto
                   {
                       FactId = fa.Id,
                       LevelLabel = lv.Value,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Value,
                   };

        axises.Add(axis);
    }

    private void AddDimension2Axis(int? dimensionId, int? levelId, List<IQueryable<AxisDto>> axises)
    {
        if (dimensionId is null) return;

        var axis = from fa in _context.Facts
                   join agg in _context.Aggregations on fa.Dimension2AggregationId equals agg.Id
                   join lv in _context.Levels on agg.LevelId equals lv.Id
                   join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                   where (dimensionId != null)
                        ? (dim.Id == dimensionId && lv.Id == levelId)
                        : (dim.Id == dimensionId)
                   select new AxisDto
                   {
                       FactId = fa.Id,
                       LevelLabel = lv.Value,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Value,
                   };

        axises.Add(axis);
    }

    private void AddDimension3Axis(int? dimensionId, int? levelId, List<IQueryable<AxisDto>> axises)
    {
        if (dimensionId is null) return;

        var axis = from fa in _context.Facts
                   join agg in _context.Aggregations on fa.Dimension3AggregationId equals agg.Id
                   join lv in _context.Levels on agg.LevelId equals lv.Id
                   join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                   where (dimensionId != null)
                        ? (dim.Id == dimensionId && lv.Id == levelId)
                        : (dim.Id == dimensionId)
                   select new AxisDto
                   {
                       FactId = fa.Id,
                       LevelLabel = lv.Value,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Value,
                   };

        axises.Add(axis);
    }

    private void AddDimension4Axis(int? dimensionId, int? levelId, List<IQueryable<AxisDto>> axises)
    {
        if (dimensionId is null) return;

        var axis = from fa in _context.Facts
                   join agg in _context.Aggregations on fa.Dimension4AggregationId equals agg.Id
                   join lv in _context.Levels on agg.LevelId equals lv.Id
                   join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                   where (dimensionId != null)
                        ? (dim.Id == dimensionId && lv.Id == levelId)
                        : (dim.Id == dimensionId)
                   select new AxisDto
                   {
                       FactId = fa.Id,
                       LevelLabel = lv.Value,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Value,
                   };

        axises.Add(axis);
    }

    private void AddAxis(List<IQueryable<AxisDto>> axises, int dimensionId, int levelId)
    {
        var axis1 = from fa in _context.Facts
                    join agg in _context.Aggregations on fa.Dimension1AggregationId equals agg.Id
                    join lv in _context.Levels on agg.LevelId equals lv.Id
                    join dim in _context.Dimensions on lv.DimensionId equals dim.Id
                    where dim.Id == dimensionId && lv.Id == levelId
                    select new AxisDto
                    {
                        FactId = fa.Id,
                        LevelLabel = agg.Value,
                        AggregationId = agg.Id,
                        DimensionId = lv.DimensionId,
                        DimensionLabel = lv.Dimension.Label,
                    };

        axises.Add(axis1);
    }
}
