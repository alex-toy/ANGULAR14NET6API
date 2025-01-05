using SoccerPlayerApi.Dtos.Structure;

namespace SoccerPlayerApi.Dtos.Levels;

public class GetLevelsResultDto : ResultDto
{
    public IEnumerable<GetLevelDto> Levels { get; set; } = new List<GetLevelDto>();
}
