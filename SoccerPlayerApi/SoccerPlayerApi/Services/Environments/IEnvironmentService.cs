using SoccerPlayerApi.Dtos.Environment;

namespace SoccerPlayerApi.Services.Environments
{
    public interface IEnvironmentService
    {
        Task<int> CreateEnvironment(EnvironmentCreateDto environment);
        Task<bool> DeleteById(int id);
        Task<EnvironmentDto?> GetEnvironmentById(int id);
        Task<IEnumerable<EnvironmentDto>> GetEnvironments();
        Task<int> UpdateAsync(EnvironmentUpdateDto environment);
    }
}