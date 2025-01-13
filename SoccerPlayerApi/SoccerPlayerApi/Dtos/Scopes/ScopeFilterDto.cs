namespace SoccerPlayerApi.Dtos.Scopes;

public class ScopeFilterDto
{
    public List<ScopeDimensionFilterDto> ScopeDimensionFilters { get; set; } = new List<ScopeDimensionFilterDto>();
}
