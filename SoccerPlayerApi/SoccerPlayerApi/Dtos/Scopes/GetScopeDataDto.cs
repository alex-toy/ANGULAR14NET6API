using SoccerPlayerApi.Dtos.Facts;

namespace SoccerPlayerApi.Dtos.Scopes;

public class GetScopeDataDto
{
    public int FactId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TimeDimensionDto TimeDimension { get; set; }
}
