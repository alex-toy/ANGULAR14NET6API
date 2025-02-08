using SoccerPlayerApi.Entities.Structure;
using System.Linq.Expressions;

namespace SoccerPlayerApi.Dtos.Facts;

public class FactCreateDto
{
    public decimal Amount { get; set; }

    public int DataTypeId { get; set; }

    public int Dimension1AggregationId { get; set; }
    public int? Dimension2AggregationId { get; set; }
    public int? Dimension3AggregationId { get; set; }
    public int? Dimension4AggregationId { get; set; }

    public int TimeAggregationId { get; set; }

    public int GetDimensionCount()
    {
        if (Dimension4AggregationId is not null && Dimension3AggregationId is not null && Dimension2AggregationId is not null) return 4;
        if (Dimension3AggregationId is not null && Dimension2AggregationId is not null) return 3;
        if (Dimension2AggregationId is not null) return 2;
        return 1;
    }

    public Expression<Func<Aggregation, bool>> ContainsAggregationId()
    {
        return aggregation => Dimension1AggregationId == aggregation.Id ||
                              Dimension2AggregationId == aggregation.Id ||
                              Dimension3AggregationId == aggregation.Id ||
                              Dimension4AggregationId == aggregation.Id;
    }
}
