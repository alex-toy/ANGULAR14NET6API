using SoccerPlayerApi.Dtos.Environment;

namespace SoccerPlayerApi.Services.Environments
{
    public interface IEnvironmentService
    {
        Task<int> CreateEnvironment(EnvironmentCreateDto environment);
        Task<EnvironmentDto?> GetEnvironmentById(int id);
        Task<IEnumerable<EnvironmentDto>> GetEnvironments();
    }
}