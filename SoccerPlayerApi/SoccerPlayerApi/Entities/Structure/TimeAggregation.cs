namespace SoccerPlayerApi.Entities.Structure;

public class TimeAggregation : Entity
{
    public int TimeLevelId { get; set; }
    public TimeLevel TimeLevel { get; set; }

    public int? MotherAggregationId { get; set; }
    public TimeAggregation? MotherAggregation { get; set; }

    public string Value { get; set; } = string.Empty;
}
