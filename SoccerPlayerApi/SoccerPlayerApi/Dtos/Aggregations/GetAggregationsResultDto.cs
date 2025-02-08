using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Aggregations;

public class GetAggregationsResultDto : ResultDto
{
    public IEnumerable<GetAggregationDto> Aggregations { get; set; } = new List<GetAggregationDto>();
}
