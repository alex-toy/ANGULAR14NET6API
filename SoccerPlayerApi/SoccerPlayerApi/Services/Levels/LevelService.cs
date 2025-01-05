using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;

namespace SoccerPlayerApi.Services.Levels;

public class LevelService : ILevelService
{
    private readonly ApplicationDbContext _context;

    public LevelService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GetLevelDto>> GetLevels(int dimensionId)
    {
        return await _context.Levels
            .Where(l => l.DimensionId == dimensionId)
            .Select(l => new GetLevelDto { Id = l.Id, Value = l.Value, DimensionId = l.DimensionId, AncestorId = l.AncestorId })
            .ToListAsync();
    }

    public async Task<int> CreateLevelAsync(CreateLevelDto level)
    {
        EntityEntry<Level> entity = await _context.Levels.AddAsync(new Level
        {
            DimensionId = level.DimensionId,
            Value = level.Value,
            AncestorId = level.AncestorId,
        });
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }
}
