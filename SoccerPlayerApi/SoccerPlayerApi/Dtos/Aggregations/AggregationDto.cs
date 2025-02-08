namespace SoccerPlayerApi.Dtos.Aggregations;

public class AggregationDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;

    public int LevelId { get; set; }
    public string LevelLabel { get; set; } = string.Empty;

    public int DimensionId { get; set; }
    public string DimensionLabel { get; set; } = string.Empty;

    public int? MotherAggregationId { get; set; }
    public string? MotherAggregationValue { get; set; }

    public int? MotherLevelId { get; set; }
    public string? MotherLevelLabel { get; set; }
}
