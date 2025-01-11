using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Entities.Structure;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Services.Dimensions;

public interface IDimensionService
{
    Task<int> CreateDimensionAsync(Dimension dimension);
    Task<int> CreateDimensionValueAsync(DimensionValueCreateDto level);
    Expression<Func<GetFactResultDto, bool>> DimensionValueFilter(List<int> dimensionValueIds);
    Task<bool> GetAreDimensionsCovered(FactCreateDto fact, int dimensionCount);
    Task<IEnumerable<DimensionDto>> GetDimensions();
    Task<IEnumerable<GetDimensionValueDto>> GetDimensionValues(int levelId);
}