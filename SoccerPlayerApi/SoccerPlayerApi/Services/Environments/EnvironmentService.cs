﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Entities.Environments;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Services.Facts;
using System.Data;

namespace SoccerPlayerApi.Services.Environments;

public class EnvironmentService : IEnvironmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IFactService _factService;
    private readonly string _connectionString;

    public EnvironmentService(ApplicationDbContext context, IFactService factService, IConfiguration configuration)
    {
        _context = context;
        _factService = factService;
        _connectionString = configuration.GetConnectionString("default")!;
    }

    public async Task<EnvironmentDto?> GetEnvironmentById(int id)
    {
        Entities.Environment? environment = await _context.Environments
            .Include(e => e.LevelFilter1)
            .Include(e => e.LevelFilter2)
            .Include(e => e.LevelFilter3)
            .Include(e => e.LevelFilter4)
            .Include(e => e.EnvironmentSortings)
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
            .Include(e => e.EnvironmentSortings)
            .ToListAsync();

        return environments.Select(x => x.ToDto());
    }

    public async Task<int> CreateEnvironment(EnvironmentCreateDto environmentDto)
    {
        CheckDimensionsNotOverlapped(environmentDto);

        Entities.Environment environmentDb = environmentDto.ToDb();

        EntityEntry<Entities.Environment> entity = await _context.Environments.AddAsync(environmentDb);
        await _context.SaveChangesAsync();
        var environmentId = entity.Entity.Id;

        await CreateRelatedEnvironmentScopes(environmentDto, environmentId);
        await CreateEnvironmentSortings(environmentDto.EnvironmentSortings, environmentId);

        return environmentId;
    }

    public async Task<bool> CreateEnvironmentSortings(IEnumerable<EnvironmentSortingDto> environmentSortings, int environmentId)
    {
        IEnumerable<EnvironmentSorting> environmentSortingDbs = environmentSortings.Select(es => es.ToDb(environmentId));

        await _context.EnvironmentSortings.AddRangeAsync(environmentSortingDbs);
        await _context.SaveChangesAsync();

        ExecuteSetEnvironmentSortingFor3Dimensions(environmentId);

        return true;
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

        await DeleteEnvironmentSortings(environment.Id);
        await CreateEnvironmentSortings(environment.EnvironmentSortings, environment.Id);

        await DeleteRelatedEnvironmentScopes(environment.Id);
        await CreateRelatedEnvironmentScopes(environment.ToCeateDto(), environment.Id);

        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    private async Task<bool> DeleteEnvironmentSortings(int environmentId)
    {
        IQueryable<EnvironmentSorting>? entity = _context.EnvironmentSortings.Where(x => x.EnvironmentId == environmentId);
        if (entity is not null) _context.EnvironmentSortings.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CreateEnvironmentScopes(IEnumerable<EnvironmentScopeDto> scopes, int environmentId)
    {
        IEnumerable<EnvironmentScope> temp = scopes.Select(s => new EnvironmentScope
        {
            EnvironmentId = environmentId,

            Dimension1Id = s.Dimension1Id,
            Dimension1AggregationId = s.Dimension1AggregationId,

            Dimension2Id = s.Dimension2Id,
            Dimension2AggregationId = s.Dimension2AggregationId,

            Dimension3Id = s.Dimension3Id,
            Dimension3AggregationId = s.Dimension3AggregationId,

            Dimension4Id = s.Dimension4Id,
            Dimension4AggregationId = s.Dimension4AggregationId,
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

    private async Task CreateRelatedEnvironmentScopes(EnvironmentCreateDto environment, int environmentId)
    {
        ScopeFilterDto filter = SetScopeFilter(environment);
        IEnumerable<EnvironmentScopeDto> scopes = await _factService.GetScopes(filter);
        await CreateEnvironmentScopes(scopes, environmentId);
    }

    private async Task<bool> DeleteRelatedEnvironmentScopes(int environmentId)
    {
        List<EnvironmentScope>? entity = await _context.EnvironmentScopes.Where(x => x.EnvironmentId == environmentId).ToListAsync();
        if (entity is not null) _context.EnvironmentScopes.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ScopeFilterDto SetScopeFilter(EnvironmentCreateDto environment)
    {
        ScopeFilterDto filter = new ScopeFilterDto();
        filter.ScopeDimensionFilters.Add(new ScopeDimensionFilterDto()
        {
            DimensionId = environment.Dimension1Id,
            LevelId = environment.LevelIdFilter1
        });

        if (environment.Dimension2Id is not null && environment.LevelIdFilter2 is not null)
        {
            filter.ScopeDimensionFilters.Add(new ScopeDimensionFilterDto()
            {
                DimensionId = environment.Dimension2Id.Value,
                LevelId = environment.LevelIdFilter2.Value
            });
        }

        if (environment.Dimension3Id is not null && environment.LevelIdFilter3 is not null)
        {
            filter.ScopeDimensionFilters.Add(new ScopeDimensionFilterDto()
            {
                DimensionId = environment.Dimension3Id.Value,
                LevelId = environment.LevelIdFilter3.Value
            });
        }

        if (environment.Dimension4Id is not null && environment.LevelIdFilter4 is not null)
        {
            filter.ScopeDimensionFilters.Add(new ScopeDimensionFilterDto()
            {
                DimensionId = environment.Dimension4Id.Value,
                LevelId = environment.LevelIdFilter4.Value
            });
        }

        return filter;
    }

    private void ExecuteSetEnvironmentSortingFor3Dimensions(int environmentId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();

        using SqlCommand command = new SqlCommand("SetEnvironmentSortingFor3Dimensions", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new SqlParameter("@EnvironmentId", SqlDbType.Int)).Value = environmentId;

        _ = command.ExecuteNonQuery();
    }
}
