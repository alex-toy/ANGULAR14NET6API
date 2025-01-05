using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactsResultDto : ResultDto
{
    public IEnumerable<GetFactResultDto> Facts { get; set; } = new List<GetFactResultDto>();
}
