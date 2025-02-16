using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities.Frames;

public class FrameScope : Entity
{
    public int FrameId { get; set; }
    public Frame Frame { get; set; }

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

    public string SortingValue { get; set; } = string.Empty;
}
