namespace SoccerPlayerApi.Entities.Structure;

public class DimensionFact
{
    public int Id { get; set; }

    public int FactId { get; set; }
    public Fact Fact { get; set; }

    public int DimensionValueId { get; set; }
    public DimensionValue DimensionValue { get; set; }
}
