namespace SoccerPlayerApi.Entities.Structure;

public class Dimension : Entity
{
    public string Value { get; set; }
    public List<Level> Levels { get; set; } = new List<Level>();
}
