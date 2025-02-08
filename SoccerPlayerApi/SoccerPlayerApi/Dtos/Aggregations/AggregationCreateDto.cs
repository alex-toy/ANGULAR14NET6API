namespace SoccerPlayerApi.Dtos.Aggregations;

public class AggregationCreateDto
{
    public int LevelId { get; set; }
    public string Value { get; set; }
    public int? MotherAggregationId { get; set; }
}
