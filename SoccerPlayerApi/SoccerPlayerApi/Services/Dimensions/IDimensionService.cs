using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(Dimension dimension);
    Task<int> CreateDimensionValueAsync(DimensionValue level);
    Task<FactCreateResultDto> CreateFactAsync(FactCreateDto level);
    Task<int> CreateLevelAsync(Level level);
    Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactDto filter);
}