using SoccerPlayerApi.Dtos.DimensionValues;

namespace SoccerPlayerApi.Services.Aggregations
{
    public interface IAggregationService
    {
        Task<int> CreateAggregation(AggregationCreateDto level);
        Task<IEnumerable<GetAggregationDto>> GetAggregations();
        Task<IEnumerable<GetAggregationDto>> GetMotherAggregations(int levelId);
    }
}