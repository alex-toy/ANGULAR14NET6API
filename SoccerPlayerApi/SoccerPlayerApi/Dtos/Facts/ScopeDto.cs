using SoccerPlayerApi.Dtos.DimensionValues;

namespace SoccerPlayerApi.Dtos.Facts;

public class ScopeDto
{
    public List<DimensionValueDto> DimensionValues { get; set; } = new List<DimensionValueDto>();
}
