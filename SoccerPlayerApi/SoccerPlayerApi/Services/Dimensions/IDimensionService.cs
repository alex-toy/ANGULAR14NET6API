using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.Facts;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(DimensionDto dimension);
    Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount);
    Task<int> GetDimensionCount();
    Task<IEnumerable<DimensionDto>> GetDimensions();
    Task<IEnumerable<GetAggregationDto>> GetDimensionValues(int levelId);
}