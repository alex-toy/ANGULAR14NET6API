using SoccerPlayerApi.Dtos.Levels;

namespace SoccerPlayerApi.Dtos.Aggregations;

public class TimeAggregationDto
{
    public int TimeAggregationId { get; set; }

    public int TimeLevelId { get; set; }
    public TimeLevelDto TimeLevel { get; set; }

    public int? MotherAggregationId { get; set; }
    public TimeAggregationDto? MotherAggregation { get; set; }

    public string Label { get; set; } = string.Empty;
}
