using SoccerPlayerApi.Dtos;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(Dimension dimension);
    Task<int> CreateDimensionValueAsync(DimensionValue level);
    Task<int> CreateFactAsync(FactCreateDto level);
    Task<int> CreateLevelAsync(Level level);
    Task Test();
}