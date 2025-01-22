namespace SoccerPlayerApi.Entities.Structure;

public class DataType : Entity
{
    public string Label { get; set; }
    public List<Fact> Facts { get; set; } = new List<Fact>();
}
