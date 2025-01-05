using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(Dimension dimension);
    Task<int> CreateDimensionValueAsync(DimensionValueCreateDto level);
    Task<FactCreateResultDto> CreateFactAsync(FactCreateDto level);
    Task<IEnumerable<DimensionDto>> GetDimensions();
    Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactFilterDto filter);
}