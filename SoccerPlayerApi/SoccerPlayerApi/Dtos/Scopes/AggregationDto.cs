namespace SoccerPlayerApi.Dtos.Scopes;

public class AggregationDto
{
    public int LevelId { get; set; }
    public string LevelLabel { get; set; } = string.Empty;
    public string Dimension { get; set; } = string.Empty;
    public int DimensionId { get; set; }
}
