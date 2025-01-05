using SoccerPlayerApi.Dtos.Levels;

namespace SoccerPlayerApi.Services.Levels
{
    public interface ILevelService
    {
        Task<int> CreateLevelAsync(CreateLevelDto level);
        Task<IEnumerable<GetLevelDto>> GetLevels(int dimensionId);
    }
}