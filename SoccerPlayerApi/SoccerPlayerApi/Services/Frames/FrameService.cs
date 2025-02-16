using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Frames;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities.Frames;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Services.Facts;
using System.Data;

namespace SoccerPlayerApi.Services.Frames;

public class FrameService : IFrameService
{
    private readonly ApplicationDbContext _context;
    private readonly IFactService _factService;
    private readonly string _connectionString;

    public FrameService(ApplicationDbContext context, IFactService factService, IConfiguration configuration)
    {
        _context = context;
        _factService = factService;
        _connectionString = configuration.GetConnectionString("default")!;
    }

    public async Task<FrameDto?> GetFrameById(int id)
    {
        Frame? environment = await _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .Include(e => e.FrameSortings)
            .FirstOrDefaultAsync(x => x.Id == id);

        return environment?.ToDto();
    }

    public async Task<IEnumerable<FrameDto>> GetFrames()
    {
        IEnumerable<Frame> environments = await _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .Include(e => e.FrameSortings)
            .ToListAsync();

        return environments.Select(x => x.ToDto());
    }

    public async Task<int> CreateFrame(FrameCreateDto environmentDto)
    {
        CheckDimensionsNotOverlapped(environmentDto);

        Frame environmentDb = environmentDto.ToDb();

        EntityEntry<Frame> entity = await _context.Environments.AddAsync(environmentDb);
        await _context.SaveChangesAsync();
        var environmentId = entity.Entity.Id;

        await CreateRelatedEnvironmentScopes(environmentDto, environmentId);
        await CreateEnvironmentSortings(environmentDto.FrameSortings, environmentId);

        return environmentId;
    }

    public async Task<bool> CreateEnvironmentSortings(IEnumerable<FrameSortingDto> environmentSortings, int environmentId)
    {
        IEnumerable<FrameSorting> environmentSortingDbs = environmentSortings.Select(es => es.ToDb(environmentId));

        await _context.EnvironmentSortings.AddRangeAsync(environmentSortingDbs);
        await _context.SaveChangesAsync();

        ExecuteSetFrameSorting(environmentId);

        return true;
    }

    public async Task<bool> DeleteById(int id)
    {
        Frame? entity = await _context.Environments.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is not null) _context.Environments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> UpdateAsync(FrameUpdateDto environment)
    {
        Frame? environmentDb = await _context.Environments.FirstOrDefaultAsync(x => x.Id == environment.Id);

        if (environmentDb is null) return -1;

        environmentDb.Map(environment);
        EntityEntry<Frame> entity = _context.Environments.Update(environmentDb);

        await DeleteRelatedEnvironmentScopes(environment.Id);
        await CreateRelatedEnvironmentScopes(environment.ToCeateDto(), environment.Id);

        await DeleteEnvironmentSortings(environment.Id);
        await CreateEnvironmentSortings(environment.FrameSortings, environment.Id);

        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    private async Task<bool> DeleteEnvironmentSortings(int environmentId)
    {
        IQueryable<FrameSorting>? entity = _context.EnvironmentSortings.Where(x => x.FrameId == environmentId);
        if (entity is not null) _context.EnvironmentSortings.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private void CheckDimensionsNotOverlapped(FrameCreateDto environment)
    {
        int levelCount = 1;
        List<int> levelIds = new()
        {
            environment.LevelIdFilter1
        };

        if (environment.LevelIdFilter2 is not null)
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

    private async Task CreateRelatedEnvironmentScopes(FrameCreateDto environment, int environmentId)
    {
        ScopeFilterDto filter = SetScopeFilter(environment);
        IEnumerable<FrameScopeDto> scopes = await _factService.GetScopes(filter);

        IEnumerable<FrameScope> environmentScopes = scopes.Select(s => new FrameScope
        {
            FrameId = environmentId,

            Dimension1Id = s.Dimension1Id,
            Dimension1AggregationId = s.Dimension1AggregationId,

            Dimension2Id = s.Dimension2Id,
            Dimension2AggregationId = s.Dimension2AggregationId,

            Dimension3Id = s.Dimension3Id,
            Dimension3AggregationId = s.Dimension3AggregationId,

            Dimension4Id = s.Dimension4Id,
            Dimension4AggregationId = s.Dimension4AggregationId,
        });

        await _context.EnvironmentScopes.AddRangeAsync(environmentScopes);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> DeleteRelatedEnvironmentScopes(int environmentId)
    {
        List<FrameScope>? entity = await _context.EnvironmentScopes.Where(x => x.FrameId == environmentId).ToListAsync();
        if (entity is not null) _context.EnvironmentScopes.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ScopeFilterDto SetScopeFilter(FrameCreateDto environment)
    {
        ScopeFilterDto filter = new ScopeFilterDto()
        {
            Dimension1Id = environment.Dimension1Id,
            Level1Id = environment.LevelIdFilter1,

            Dimension2Id = environment.Dimension2Id,
            Level2Id = environment.LevelIdFilter2,

            Dimension3Id = environment.Dimension3Id,
            Level3Id = environment.LevelIdFilter3,

            Dimension4Id = environment.Dimension4Id,
            Level4Id = environment.LevelIdFilter4,
        };

        return filter;
    }

    private void ExecuteSetFrameSorting(int environmentId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand command = new SqlCommand("SetFrameSorting", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter("@frameId", SqlDbType.Int)).Value = environmentId;

        _ = command.ExecuteNonQuery();
    }
}
