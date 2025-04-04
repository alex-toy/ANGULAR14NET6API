﻿using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Bos;
using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities.Frames;
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

    public async Task<IEnumerable<FrameScopeDto>> GetScopes(ScopeFilterDto scopeFilter)
    {
        List<int> dimensionIds = _context.Dimensions.Select(d => d.Id).ToList();
        int dimensionCount = dimensionIds.Count();
        List<IQueryable<AxisDto>> axises = new List<IQueryable<AxisDto>>();

        AddDimension1Axis(scopeFilter.Dimension1Id, scopeFilter.Level1Id, axises);
        AddDimension2Axis(scopeFilter.Dimension2Id, scopeFilter.Level2Id, axises);
        AddDimension3Axis(scopeFilter.Dimension3Id, scopeFilter.Level3Id, axises);
        AddDimension4Axis(scopeFilter.Dimension4Id, scopeFilter.Level4Id, axises);

        IQueryable<FrameScopeDto> result = JoinAxes(axises, dimensionCount);

        List<FrameScopeDto> distinctResult = await result.Distinct().ToListAsync();
        return distinctResult ?? new List<FrameScopeDto>();
    }

    public async Task<IEnumerable<FrameScopeDto>> GetScopesByFrameId(int environmentId)
    {
        List<FrameScope>? environmentScopes = await _context.EnvironmentScopes
                                                                        .Include(es => es.Dimension1Aggregation)
                                                                        .Include(es => es.Dimension2Aggregation)
                                                                        .Include(es => es.Dimension3Aggregation)
                                                                        .Include(es => es.Dimension4Aggregation)
                                                                        .Where(d => d.FrameId == environmentId)
                                                                        .ToListAsync();

        if (environmentScopes is null) throw new Exception("environment scopes don't exist");

        IEnumerable<FrameScopeDto> scopes = environmentScopes.Select(es => ExtractScopeDto(es));

        return scopes ?? new List<FrameScopeDto>();
    }

    private static FrameScopeDto ExtractScopeDto(FrameScope es)
    {
        if (
            es.Dimension4Id is not null && es.Dimension4AggregationId is not null && es.Dimension4Aggregation is not null &&
            es.Dimension3Id is not null && es.Dimension3AggregationId is not null && es.Dimension3Aggregation is not null &&
            es.Dimension2Id is not null && es.Dimension2AggregationId is not null && es.Dimension2Aggregation is not null
        )
        {
            return new FrameScopeDto
            {
                Dimension1Id = es.Dimension1Id,
                Dimension1AggregationId = es.Dimension1AggregationId,
                Level1Label = es.Dimension1Aggregation.Level.Label,
                Dimension1AggregationLabel = es.Dimension1Aggregation.Label,

                Dimension2Id = es.Dimension2Id,
                Dimension2AggregationId = es.Dimension2AggregationId,
                Level2Label = es.Dimension2Aggregation.Level.Label,
                Dimension2AggregationLabel = es.Dimension2Aggregation.Label,

                Dimension3Id = es.Dimension3Id,
                Dimension3AggregationId = es.Dimension3AggregationId,
                Level3Label = es.Dimension3Aggregation.Level.Label,
                Dimension3AggregationLabel = es.Dimension3Aggregation.Label,

                Dimension4Id = es.Dimension4Id,
                Dimension4AggregationId = es.Dimension4AggregationId,
                Level4Label = es.Dimension4Aggregation.Level.Label,
                Dimension4AggregationLabel = es.Dimension4Aggregation.Label,

                SortingValue = es.SortingValue
            };
        }

        if (
            es.Dimension3Id is not null && es.Dimension3AggregationId is not null && es.Dimension3Aggregation is not null &&
            es.Dimension2Id is not null && es.Dimension2AggregationId is not null && es.Dimension2Aggregation is not null
        )
        {
            return new FrameScopeDto
            {
                Dimension1Id = es.Dimension1Id,
                Dimension1AggregationId = es.Dimension1AggregationId,
                Level1Label = es.Dimension1Aggregation.Label,
                Dimension1AggregationLabel = es.Dimension1Aggregation.Label,

                Dimension2Id = es.Dimension2Id,
                Dimension2AggregationId = es.Dimension2AggregationId,
                Level2Label = es.Dimension2Aggregation.Label,
                Dimension2AggregationLabel = es.Dimension2Aggregation.Label,

                Dimension3Id = es.Dimension3Id,
                Dimension3AggregationId = es.Dimension3AggregationId,
                Level3Label = es.Dimension3Aggregation.Label,
                Dimension3AggregationLabel = es.Dimension3Aggregation.Label,

                SortingValue = es.SortingValue
            };
        }

        if (es.Dimension2Id is not null && es.Dimension2AggregationId is not null && es.Dimension2Aggregation is not null)
        {
            return new FrameScopeDto
            {
                Dimension1Id = es.Dimension1Id,
                Dimension1AggregationId = es.Dimension1AggregationId,
                Level1Label = es.Dimension1Aggregation.Label,
                Dimension1AggregationLabel = es.Dimension1Aggregation.Label,

                Dimension2Id = es.Dimension2Id,
                Dimension2AggregationId = es.Dimension2AggregationId,
                Level2Label = es.Dimension2Aggregation.Label,
                Dimension2AggregationLabel = es.Dimension2Aggregation.Label,

                SortingValue = es.SortingValue
            };
        }

        return new FrameScopeDto
        {
            Dimension1Id = es.Dimension1Id,
            Dimension1AggregationId = es.Dimension1AggregationId,
            Level1Label = es.Dimension1Aggregation.Label,
            Dimension1AggregationLabel = es.Dimension1Aggregation.Label,

            SortingValue = es.SortingValue
        };
    }

    public async Task<IEnumerable<GetScopeDataDto>> GetScopeData(FrameScopeDto scope)
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

    public async Task<Dictionary<int, List<GetScopeDataDto>>> GetScopeDataTest(FrameScopeDto scope)
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

        var facts = await resultQuery.ToListAsync();

        return facts.ToLookup(f => f.DataTypeId).ToDictionary(group => group.Key, group => group.ToList());
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
                    Label = fact.Aggregation1.Label,
                    LevelId = fact.Aggregation1.Level.Id,
                    LevelLabel = fact.Aggregation1.Level.Label,
                    DimensionId = fact.Aggregation1.Level.DimensionId,
                    DimensionLabel = fact.Aggregation1.Level.Dimension.Label,
                }
            });

        if (filter.DataTypeId is not null) facts = facts.Where(fact => fact.DataTypeId == filter.DataTypeId);

        facts = facts.Where(filter.FactIsAtLevels());

        facts = facts.Where(filter.FactIsInAggregations());

        return await facts.ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTimeSerie(GetFactFilterDto filter)
    {
        var facts = _context.Facts
            .Where(f => f.TimeAggregationId == filter.TimeAggregationId)
            .Include(f => f.Aggregation1)
            .Include(f => f.Aggregation2)
            .Include(f => f.Aggregation3)
            .Include(f => f.Aggregation4)
            .Where(f => f.Dimension1AggregationId == filter.Dimension1Id && 
                        f.Dimension2AggregationId == filter.Dimension2Id && 
                        f.Dimension3AggregationId == filter.Dimension3Id && 
                        f.Dimension4AggregationId == filter.Dimension4Id &&
                        f.DataTypeId == filter.DataTypeId &&
                        f.TimeAggregation.TimeLevelId== filter.TimeLevelId)
            .Select(fact => new
            {
                Id = fact.Id,
                Amount = fact.Amount,
                DataTypeId = fact.DataTypeId,
                TimeLabel = fact.TimeAggregation.Label
            });

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

        return await _context.TimeAggregations.Where(a => string.Compare(a.Label, presentDate) <= 0 && a.TimeLevelId == levelId)
                                               .OrderByDescending(a => a.Label)
                                               .Take(span)
                                               .Select(agg => new TimeAggregationDto
                                               {
                                                   TimeAggregationId = agg.Id,
                                                   Label = agg.Label
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

    private static IQueryable<FrameScopeDto> JoinAxes(List<IQueryable<AxisDto>> axises, int dimensionCount)
    {
        if (dimensionCount == 1) return JoinAxisFor1Dimensions(axises);

        if (dimensionCount == 2) return JoinAxisFor2Dimensions(axises);

        if (dimensionCount == 3) return JoinAxisFor3Dimensions(axises);

        return JoinAxisFor4Dimensions(axises);
    }

    private static IQueryable<FrameScopeDto> JoinAxisFor4Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               join d3 in axises[2] on d1.FactId equals d3.FactId
               join d4 in axises[3] on d1.FactId equals d4.FactId
               select new FrameScopeDto
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

    private static IQueryable<FrameScopeDto> JoinAxisFor3Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               join d3 in axises[2] on d1.FactId equals d3.FactId
               select new FrameScopeDto
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

    private static IQueryable<FrameScopeDto> JoinAxisFor2Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               join d2 in axises[1] on d1.FactId equals d2.FactId
               select new FrameScopeDto
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

    private static IQueryable<FrameScopeDto> JoinAxisFor1Dimensions(List<IQueryable<AxisDto>> axises)
    {
        return from d1 in axises[0]
               select new FrameScopeDto
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_1_Dimensions(FrameScopeDto scope)
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
                              DataTypeId = d1.TypeId,
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_2_Dimensions(FrameScopeDto scope)
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
                              DataTypeId = d1.TypeId,
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_3_Dimensions(FrameScopeDto scope)
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
                              DataTypeId = d1.TypeId,
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

    private IQueryable<GetScopeDataDto> GetScopeDataFor_4_Dimensions(FrameScopeDto scope)
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
                              DataTypeId = d1.TypeId,
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

    private IQueryable<AxisBo> GetDimension1Axis(FrameScopeDto scope)
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
                   Value = lv.Label,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Label,
                   AggId = agg.Id
               };
    }

    private IQueryable<AxisBo> GetDimension2Axis(FrameScopeDto scope)
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
                   Value = lv.Label,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Label,
                   AggId = agg.Id
               };
    }

    private IQueryable<AxisBo> GetDimension3Axis(FrameScopeDto scope)
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
                   Value = lv.Label,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Label,
                   AggId = agg.Id
               };
    }

    private IQueryable<AxisBo> GetDimension4Axis(FrameScopeDto scope)
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
                   Value = lv.Label,
                   DimensionId = prod.Id,
                   AggregationValue = agg.Label,
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
                   AggregationValue = dv.Label,
                   AggId = dv.Id
               };
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
                       LevelLabel = lv.Label,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Label,
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
                       LevelLabel = lv.Label,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Label,
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
                       LevelLabel = lv.Label,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Label,
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
                       LevelLabel = lv.Label,
                       AggregationId = agg.Id,
                       DimensionId = lv.DimensionId,
                       DimensionLabel = lv.Dimension.Label,
                       AggregationLabel = agg.Label,
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
                        LevelLabel = agg.Label,
                        AggregationId = agg.Id,
                        DimensionId = lv.DimensionId,
                        DimensionLabel = lv.Dimension.Label,
                    };

        axises.Add(axis1);
    }
}
