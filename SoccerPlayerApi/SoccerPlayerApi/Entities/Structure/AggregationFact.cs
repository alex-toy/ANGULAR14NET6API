namespace SoccerPlayerApi.Entities.Structure;

public class AggregationFact
{
    public int Id { get; set; }

    public int FactId { get; set; }
    public Fact Fact { get; set; }

    public int AggregationId { get; set; }
    public Aggregation Aggregation { get; set; }
}
