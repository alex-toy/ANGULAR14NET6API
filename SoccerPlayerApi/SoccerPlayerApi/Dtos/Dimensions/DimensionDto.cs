using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Dtos.Dimensions;

public class DimensionDto : ResultDto
{
    public int Id { get; set; }
    public string Label { get; set; }
    public List<GetLevelDto> Levels { get; set; } = new List<GetLevelDto>();

    public Dimension ToDb()
    {
        return new Dimension
        {
            Id = Id,
            Label = Label,
        };
    }
}
