using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities;
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
            .ToListAsync();

        return environments.Select(x => x.ToDto());
    }

    public async Task<int> CreateEnvironment(EnvironmentCreateDto environment)
    {
        CheckDimensionsNotOverlapped(environment);

        Entities.Environment environmentDb = environment.ToDb();

        EntityEntry<Entities.Environment> entity = await _context.Environments.AddAsync(environmentDb);

        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<bool> CreateEnvironmentScopes(IEnumerable<ScopeDto> scopes, int environmentId)
    {
        IEnumerable<EnvironmentScope> temp = scopes.Select(s => new EnvironmentScope
        {
            EnvironmentId = environmentId,

            Dimension1Id = s.Aggregations.First().DimensionId,
            Dimension1AggregationId = s.Aggregations.First().AggregationId,

            Dimension2Id = s.Aggregations.Count() >= 2 ? s.Aggregations.ElementAt(1).DimensionId : null,
            Dimension2AggregationId = s.Aggregations.Count() >= 2 ? s.Aggregations.ElementAt(1)?.AggregationId : null,

            Dimension3Id = s.Aggregations.Count() >= 3 ? s.Aggregations.ElementAt(2).DimensionId : null,
            Dimension3AggregationId = s.Aggregations.Count() >= 3 ? s.Aggregations.ElementAt(2)?.AggregationId : null,

            Dimension4Id = s.Aggregations.Count() >= 4 ? s.Aggregations.ElementAt(3).DimensionId : null,
            Dimension4AggregationId = s.Aggregations.Count() >= 4 ? s.Aggregations.ElementAt(3)?.AggregationId : null,
        });

        await _context.EnvironmentScopes.AddRangeAsync(temp);

        await _context.SaveChangesAsync();
        return true;
    }

    private void CheckDimensionsNotOverlapped(EnvironmentCreateDto environment)
    {
        int levelCount = 1;
        List<int> levelIds = new()
        {
            environment.LevelIdFilter1
        };

        if (environment.LevelIdFilter2.HasValue)
        {
            levelCount++;
            levelIds.Add(environment.LevelIdFilter2.Value);
        }

        if (environment.LevelIdFilter3.HasValue)
        {
            levelCount++;
            levelIds.Add(environment.LevelIdFilter3.Value);
        }

        if (environment.LevelIdFilter4.HasValue)
        {
            levelCount++;
            levelIds.Add(environment.LevelIdFilter4.Value);
        }

        var dimensionIds = _context.Levels.Include(x => x.Dimension)
                                          .Where(x => levelIds.Contains(x.Id))
                                          .Select(x => x.DimensionId)
                                          .Distinct()
                                          .Count();

        if (levelCount > dimensionIds) throw new Exception("dimensions are overlapping");
    }

    public async Task<bool> DeleteById(int id)
    {
        Entities.Environment? entity = await _context.Environments.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is not null) _context.Environments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> UpdateAsync(EnvironmentUpdateDto environment)
    {
        Entities.Environment? environmentDb = await _context.Environments.FirstOrDefaultAsync(x => x.Id == environment.Id);

        if (environmentDb is null) return -1;

        environmentDb.Map(environment);
        EntityEntry<Entities.Environment> entity = _context.Environments.Update(environmentDb);

        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }
}
