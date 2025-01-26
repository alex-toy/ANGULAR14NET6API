using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Dtos.Scopes;

public class AggregationDto
{
    public int AggregationId { get; set; }
    public string LevelLabel { get; set; } = string.Empty;
    public string Dimension { get; set; } = string.Empty;
    public int DimensionId { get; set; }
}
