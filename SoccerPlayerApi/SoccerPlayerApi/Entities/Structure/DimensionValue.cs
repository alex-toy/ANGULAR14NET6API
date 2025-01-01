namespace SoccerPlayerApi.Entities.Structure;

public class DimensionValue
{
    public int Id { get; set; }

    public int LevelId { get; set; }
    public Level Level { get; set; }

    public string Value { get; set; }
}
