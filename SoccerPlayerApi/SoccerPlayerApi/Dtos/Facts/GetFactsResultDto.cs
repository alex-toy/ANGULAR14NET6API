using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactsResultDto : ResultDto
{
    public IEnumerable<FactDto> Facts { get; set; } = new List<FactDto>();
}
