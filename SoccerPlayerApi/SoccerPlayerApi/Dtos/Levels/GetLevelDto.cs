using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Dtos.Levels;

public class GetLevelDto
{
    public int Id { get; set; }
    public string Label { get; set; }
    public int DimensionId { get; set; }
    public int? AncestorId { get; set; }

    public Level ToDb()
    {
        return new Level
        {
            Id = Id,
            Value = Label,
            DimensionId = DimensionId,
            AncestorId = AncestorId,
        };
    }
}
