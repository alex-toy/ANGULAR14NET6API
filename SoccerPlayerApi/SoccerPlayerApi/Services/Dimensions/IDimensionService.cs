using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(Dimension dimension);
    Task<int> CreateDimensionValueAsync(DimensionValueCreateDto level);
    Task<FactCreateResultDto> CreateFactAsync(FactCreateDto level);
    Task<int> CreateLevelAsync(CreateLevelDto level);
    Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactDto filter);
}