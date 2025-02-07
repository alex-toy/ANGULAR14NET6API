using Microsoft.EntityFrameworkCore;

namespace SoccerPlayerApi.Entities.Structure;

public class Fact : Entity
{
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public int DataTypeId { get; set; }
    public DataType DataType { get; set; }

    public int Dimension1AggregationId { get; set; }
    public Aggregation Aggregation1 { get; set; }

    public int? Dimension2AggregationId { get; set; }
    public Aggregation? Aggregation2 { get; set; }

    public int? Dimension3AggregationId { get; set; }
    public Aggregation? Aggregation3 { get; set; }

    public int? Dimension4AggregationId { get; set; }
    public Aggregation? Aggregation4 { get; set; }

    public int TimeAggregationId { get; set; }
    public TimeAggregation TimeAggregation { get; set; }

}
