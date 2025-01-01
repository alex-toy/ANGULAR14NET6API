namespace SoccerPlayerApi.Entities.Structure;

public class Dimension
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Level { get; set; }

    public int AncestorId { get; set; }
    public Dimension? Ancestor { get; set; }
}
