using Microsoft.EntityFrameworkCore;

namespace SoccerPlayerApi.Entities.Structure;

public class Fact : Entity
{
    public int DataTypeId { get; set; }
    public DataType DataType { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public List<AggregationFact> DimensionFacts { get; set; }
}
