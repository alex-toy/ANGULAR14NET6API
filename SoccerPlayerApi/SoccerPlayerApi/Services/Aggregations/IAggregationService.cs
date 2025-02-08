using SoccerPlayerApi.Dtos.Aggregations;

namespace SoccerPlayerApi.Services.Aggregations
{
    public interface IAggregationService
    {
        Task<int> CreateAggregation(AggregationCreateDto level);
        Task<IEnumerable<GetAggregationDto>> GetAggregations();
        Task<IEnumerable<GetAggregationDto>> GetMotherAggregations(int levelId);
    }
}