namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactFilterDto
{
    public int? DataTypeId { get; set; }
    public List<GetFactDimensionFilterDto> FactDimensionFilters { get; set; } = new List<GetFactDimensionFilterDto>();

    public int Dimension1AggregationId { get; set; }
    public int? Dimension2AggregationId { get; set; }
    public int? Dimension3AggregationId { get; set; }
    public int? Dimension4AggregationId { get; set; }

    public int TimeAggregationId { get; set; }
}
