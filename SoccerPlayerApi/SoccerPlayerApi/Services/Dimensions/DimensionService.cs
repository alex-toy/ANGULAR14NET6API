using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using System;

namespace SoccerPlayerApi.Services.Dimensions;

public class DimensionService : IDimensionService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Dimension> _dimensionRepo;
    private readonly IGenericRepo<DimensionValue> _dimensionValueRepo;
    private readonly IGenericRepo<Fact> _factRepo;

    public DimensionService(ApplicationDbContext context, IGenericRepo<Dimension> dimensionRepo, IGenericRepo<Fact> factRepo, IGenericRepo<DimensionValue> dimensionValueRepo)
    {
        _context = context;
        _dimensionRepo = dimensionRepo;
        _factRepo = factRepo;
        _dimensionValueRepo = dimensionValueRepo;
    }

    public async Task<int> CreateFactAsync(FactCreateDto fact)
    {
        var factDb = new Fact { Amount = fact.Amount, Type = fact.Type };
        int entityId = await _factRepo.CreateAsync(factDb);

        IEnumerable<DimensionFact> dimensionFacts = fact.DimensionFacts.Select(x => new DimensionFact { 
            DimensionValueId = x.DimensionValueId, 
            FactId = entityId,
        });

        factDb.DimensionFacts = dimensionFacts.ToList();

        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateDimensionAsync(Dimension dimension)
    {
        int entityId = await _dimensionRepo.CreateAsync(dimension);
        await _context.SaveChangesAsync();
        return entityId;
    }

    public async Task<int> CreateLevelAsync(Level level)
    {
        EntityEntry<Level> entity = await _context.Levels.AddAsync(level);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<int> CreateDimensionValueAsync(DimensionValue level)
    {
        EntityEntry<DimensionValue> entity = await _context.DimensionValues.AddAsync(level);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<IEnumerable<Fact>> GetFacts()
    {
        IEnumerable<Fact> entity = await _factRepo.GetAllAsync(x => true);
        return entity;
    }
}
