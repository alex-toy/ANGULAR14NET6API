using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;

namespace SoccerPlayerApi.Services.Environments;

public class EnvironmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IGenericRepo<Entities.Environment> _environmentRepo;

    public EnvironmentService(ApplicationDbContext context, IGenericRepo<Entities.Environment> repo)
    {
        _context = context;
        _environmentRepo = repo;
    }

    public async Task<IEnumerable<EnvironmentDto>> GetEnvironments()
    {
        IEnumerable<Entities.Environment> environments = await _environmentRepo.GetAllAsync(x => true);
        return environments.Select(x => new EnvironmentDto { });
    }
}
