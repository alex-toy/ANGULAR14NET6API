using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Dtos.Facts;

public class FactDto
{
    public int Id { get; set; }

    public int DataTypeId { get; set; }
    public DataType DataType { get; set; }

    public decimal Amount { get; set; }


    public AggregationDto Aggregation1 { get; set; }

    public AggregationDto? Aggregation2 { get; set; }

    public AggregationDto? Aggregation3 { get; set; }

    public AggregationDto? Aggregation4 { get; set; }


    public int TimeAggregationId { get; set; }
    public TimeAggregation TimeAggregation { get; set; }
}
