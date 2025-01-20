namespace SoccerPlayerApi.Dtos.Scopes;

public class TimeDimensionDto
{
    public int TimeLevelId { get; set; }
    public int TimeAggregationId { get; set; }
    public string TimeAggregationLabel { get; set; } = string.Empty;
    public string TimeAggregationValue { get; set; } = string.Empty;
}
