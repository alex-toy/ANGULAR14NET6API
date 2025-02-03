using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Services.Aggregations;
using SoccerPlayerApi.Utils;

namespace SoccerPlayerApi.Services.Levels;

public class LevelService : ILevelService
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregationService _aggregationService;

    public LevelService(ApplicationDbContext context, IAggregationService aggregationService)
    {
        _context = context;
        _aggregationService = aggregationService;
    }

    public async Task<IEnumerable<GetLevelDto>> GetLevels(int dimensionId)
    {
        return await _context.Levels
            .Where(l => l.DimensionId == dimensionId)
            .Select(l => new GetLevelDto { Id = l.Id, Value = l.Value, DimensionId = l.DimensionId, AncestorId = l.AncestorId })
            .ToListAsync();
    }

    public async Task<IEnumerable<GetLevelDto>> GetTimeLevels()
    {
        return await _context.TimeLevels
            .Select(l => new GetLevelDto { Id = l.Id, Value = l.Value, AncestorId = l.AncestorId })
            .ToListAsync();
    }

    public async Task<IEnumerable<GetDimensionLevelDto>> GetDimensionLevels()
    {
        return await _context.Dimensions
            .Include(d => d.Levels)
            .Select(d => new GetDimensionLevelDto() { 
                DimensionId = d.Id, 
                Value = d.Value, 
                Levels = d.Levels.Select(l => new GetLevelDto() { Id = l.Id, Value = l.Value, AncestorId = l.AncestorId })
            })
            .ToListAsync();
    }

    public async Task<int> CreateLevelAsync(CreateLevelDto level)
    {
        Level? existingLabel = _context.Levels.FirstOrDefault(l => l.DimensionId == level.DimensionId && l.AncestorId == level.AncestorId);
        if (existingLabel is not null) throw new Exception("there is an existing level for this dimension");

        EntityEntry<Level> entity = await _context.Levels.AddAsync(new Level
        {
            DimensionId = level.DimensionId,
            Value = level.Value,
            AncestorId = level.AncestorId,
        });

        await _context.SaveChangesAsync();

        if (level.AncestorId is null)
        {
            await _aggregationService.CreateAggregation(new AggregationCreateDto { 
                LevelId = entity.Entity.Id,
                Value = level.Value,
                MotherAggregationId = null
            });
        }
        return entity.Entity.Id;
    }
}
