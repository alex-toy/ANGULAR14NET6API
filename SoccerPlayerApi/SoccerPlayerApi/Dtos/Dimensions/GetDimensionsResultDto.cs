using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Dimensions;

public class GetDimensionsResultDto : ResultDto
{
    public IEnumerable<DimensionDto> Dimensions { get; set; } = Enumerable.Empty<DimensionDto>();
}
