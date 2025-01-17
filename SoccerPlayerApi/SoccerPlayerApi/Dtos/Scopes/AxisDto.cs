namespace SoccerPlayerApi.Dtos.Scopes;

public class AxisDto
{
    public int AggregationId { get; set; }
    public int FactId { get; set; }
    public string LevelLabel { get; set; } = string.Empty;
    public string DimensionLabel { get; set; } = string.Empty;
    public int DimensionId { get; set; }
}
