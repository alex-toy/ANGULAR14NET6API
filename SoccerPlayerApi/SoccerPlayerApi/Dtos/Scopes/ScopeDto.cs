namespace SoccerPlayerApi.Dtos.Scopes;

public class ScopeDto
{
    public string LevelIds { get; set; } = string.Empty;
    public List<DimensionValueDto> DimensionValues { get; set; } = new List<DimensionValueDto>();
}
