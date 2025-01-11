using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactTypesResultDto : ResultDto
{
    public IEnumerable<string> Types { get; set; } = new List<string>();
}
