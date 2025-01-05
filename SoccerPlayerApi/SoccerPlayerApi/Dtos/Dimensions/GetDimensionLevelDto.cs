using SoccerPlayerApi.Dtos.Levels;

namespace SoccerPlayerApi.Dtos.Dimensions;

public class GetDimensionLevelDto
{
    public int DimensionId { get; set; }
    public string Value { get; set; }
    public IEnumerable<GetLevelDto> Levels { get; set; } = new List<GetLevelDto>();
}
