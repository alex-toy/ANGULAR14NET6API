namespace SoccerPlayerApi.Entities.Structure;

public class DimensionValue<T> where T : Dimension
{
    public int Id { get; set; }
    public string Type { get; set; }

    public int DimensionId { get; set; }
    public T Dimension { get; set; }

    public string Value { get; set; }
}
