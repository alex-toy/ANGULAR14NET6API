using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Dimensions;

public class GetDimensionLevelsResultDto : ResultDto
{
    public IEnumerable<GetDimensionLevelDto> DimensionLevels { get; set; } = new List<GetDimensionLevelDto>();
}
