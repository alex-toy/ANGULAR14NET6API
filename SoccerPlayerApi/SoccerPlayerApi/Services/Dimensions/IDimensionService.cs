using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(DimensionDto dimension);
    Task<int> CreateDimensionValueAsync(AggregationCreateDto level);
    Expression<Func<GetFactResultDto, bool>> DimensionValueFilter(List<int> dimensionValueIds);
    Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount);
    Task<int> GetDimensionCount();
    Task<IEnumerable<DimensionDto>> GetDimensions();
    Task<IEnumerable<GetAggregationDto>> GetDimensionValues(int levelId);
}