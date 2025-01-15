namespace SoccerPlayerApi.Entities.Structure;

public class Level
{
    public int Id { get; set; }

    public string Value { get; set; }

    public int DimensionId { get; set; }
    public Dimension Dimension { get; set; }

    public int? AncestorId { get; set; }
    public Level? Ancestor { get; set; }

    public List<Level> Children { get; set; } = new List<Level>();
    public List<Aggregation> DimensionValues { get; set; } = new List<Aggregation>();
}
