namespace SoccerPlayerApi.Entities.Structure;

public class Aggregation : Entity
{
    public int LevelId { get; set; }
    public Level Level { get; set; }

    public string Value { get; set; } = string.Empty;
}
