using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities.Environments;

public class EnvironmentScope : Entity
{
    public int EnvironmentId { get; set; }
    public Environment Environment { get; set; }

    public int Dimension1Id { get; set; }
    public int Dimension1AggregationId { get; set; }
    public Aggregation Dimension1Aggregation { get; set; }

    public int? Dimension2Id { get; set; }
    public int? Dimension2AggregationId { get; set; }
    public Aggregation? Dimension2Aggregation { get; set; }

    public int? Dimension3Id { get; set; }
    public int? Dimension3AggregationId { get; set; }
    public Aggregation? Dimension3Aggregation { get; set; }

    public int? Dimension4Id { get; set; }
    public int? Dimension4AggregationId { get; set; }
    public Aggregation? Dimension4Aggregation { get; set; }
}
