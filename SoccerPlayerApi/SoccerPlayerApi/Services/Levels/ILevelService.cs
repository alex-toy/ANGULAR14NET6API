using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.Levels;

namespace SoccerPlayerApi.Services.Levels
{
    public interface ILevelService
    {
        Task<int> CreateLevelAsync(CreateLevelDto level);
        Task<IEnumerable<GetDimensionLevelDto>> GetDimensionLevels();
        Task<IEnumerable<GetLevelDto>> GetLevels(int dimensionId);
    }
}