using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Simulations;
using SoccerPlayerApi.Repo;

namespace SoccerPlayerApi.Services.Simulations;

public class SimulationService : ISimulationService
{
    private readonly ApplicationDbContext _context;

    public SimulationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AlgorithmDto>> GetAlgorithms()
    {
        return await _context.Algorithms
                                .Include(a => a.Keys)
                                .Select(x => new AlgorithmDto { 
                                    Label = x.Label, 
                                    Keys = x.Keys.Select(x => new AlgorithmParameterKeyDto {
                                        AlgorithmParameterKeyId = x.Id,
                                        Value = x.Key, 
                                        AlgorithmId = x.AlgorithmId}).ToList() 
                                })
                                .ToListAsync();
    }
}
