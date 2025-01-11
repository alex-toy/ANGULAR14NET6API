using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.DimensionValues;

public class GetDimensionValuesResultDto : ResultDto
{
    public IEnumerable<GetDimensionValueDto> DimensionValues { get; set; } = new List<GetDimensionValueDto>();
}
