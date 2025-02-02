namespace SoccerPlayerApi.Dtos.DimensionValues;

public class GetAggregationDto
{
    public int Id { get; set; }

    public int LevelId { get; set; }
    public string Value { get; set; }

    public int? MotherAggregationId { get; set; }
    public string? MotherAggregationValue { get; set; }
}
