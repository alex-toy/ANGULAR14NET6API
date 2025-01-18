using SoccerPlayerApi.Dtos.Environment;

namespace SoccerPlayerApi.Services.Environments
{
    public interface IEnvironmentService
    {
        Task<EnvironmentDto?> GetEnvironmentById(int id);
        Task<IEnumerable<EnvironmentDto>> GetEnvironments();
    }
}