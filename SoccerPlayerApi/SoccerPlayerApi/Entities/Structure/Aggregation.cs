namespace SoccerPlayerApi.Entities.Structure;

public class Aggregation : Entity
{
    public int LevelId { get; set; }
    public Level Level { get; set; }

    public int? MotherAggregationId { get; set; }
    public Aggregation? MotherAggregation { get; set; }

    public string Label { get; set; } = string.Empty;
}
