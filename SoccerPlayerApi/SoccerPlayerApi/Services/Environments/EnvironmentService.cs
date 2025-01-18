using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Repo;

namespace SoccerPlayerApi.Services.Environments;

public class EnvironmentService : IEnvironmentService
{
    private readonly ApplicationDbContext _context;

    public EnvironmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EnvironmentDto?> GetEnvironmentById(int id)
    {
        Entities.Environment? environment = await _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .Include(e => e.LevelFilter5)
            .FirstOrDefaultAsync(x => x.Id == id);

        return environment?.ToDto();
    }

    public async Task<IEnumerable<EnvironmentDto>> GetEnvironments()
    {
        IEnumerable<Entities.Environment> environments = await _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .Include(e => e.LevelFilter5)
            .ToListAsync();

        return environments.Select(x => x.ToDto());
    }
}
