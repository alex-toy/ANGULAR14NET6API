namespace SoccerPlayerApi.Entities.Structure;

public class Dimension : Entity
{
    public string Label { get; set; }
    public List<Level> Levels { get; set; } = new List<Level>();
}
