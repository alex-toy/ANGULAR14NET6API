using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Levels;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly ILevelService _levelService;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, ILevelService levelService)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _levelService = levelService;
    }

    public async Task<IEnumerable<DimensionDto>> GetDimensions()
    {
        return await _context.Dimensions
            .Include(d => d.Levels)
            .Select(d => new DimensionDto { Id = d.Id, Label = d.Label, Levels = d.Levels.Select(x => x.ToDto()).ToList() })
            .ToListAsync();
    }

    public async Task<int> GetDimensionCount()
    {
        return await _context.Dimensions.CountAsync();
    }

    public async Task<int> CreateDimensionAsync(DimensionDto dimension)
    {
        Dimension? dimensionTest = _context.Dimensions.FirstOrDefault(d => d.Label == dimension.Label);
        if (dimensionTest is not null) throw new Exception("Dimension value should be unique");

        int entityId = await _dimensionRepo.CreateAsync(dimension.ToDb());

        await _levelService.CreateLevelAsync(new CreateLevelDto { Label = $"all-{dimension.Label}", DimensionId = entityId, AncestorId = null });

        await _context.SaveChangesAsync();
        return entityId;
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
            .Where(fact.ContainsAggregationId())
            .Include(x => x.Level).ThenInclude(x => x.Dimension)
            .Select(x => x.Level.Dimension.Id)
            .Distinct()
            .ToListAsync();
        return distinctDimensionValueIds.Count() == dimensionCount;
    }
}
