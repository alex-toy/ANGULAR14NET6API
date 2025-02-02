using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;

namespace SoccerPlayerApi.Services.Aggregations;

public class AggregationService : IAggregationService
{
    private readonly ApplicationDbContext _context;

    public AggregationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GetAggregationDto>> GetAggregations()
    {
        return await _context.Aggregations
            .Include(a => a.MotherAggregation)
            .Select(agg => new GetAggregationDto { 
                LevelId = agg.Id, 
                Value = agg.Value,
                MotherAggregationId = agg.MotherAggregationId,
                MotherAggregationValue = agg.MotherAggregation != null ? agg.MotherAggregation.Value : string.Empty
            })
            .ToListAsync();
    }

    public async Task<int> CreateAggregation(AggregationCreateDto aggregation)
    {
        Level? level = _context.Levels.FirstOrDefault(x => x.Id == aggregation.LevelId);
        if (level is null) throw new Exception("invalid level");

        if (aggregation.MotherAggregationId is not null)
        {
            Aggregation? motherAggregation = _context.Aggregations.Include(x => x.Level).FirstOrDefault(x => x.Id == aggregation.MotherAggregationId);
            if (motherAggregation is null) throw new Exception("invalid mother aggregation");
            
            if (motherAggregation.LevelId != level.AncestorId) throw new Exception("invalid ancestor level");
        }

        EntityEntry<Aggregation> entity = await _context.Aggregations.AddAsync(new Aggregation
        {
            LevelId = aggregation.LevelId,
            Value = aggregation.Value,
            MotherAggregationId = aggregation.MotherAggregationId,
        });
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }
}
