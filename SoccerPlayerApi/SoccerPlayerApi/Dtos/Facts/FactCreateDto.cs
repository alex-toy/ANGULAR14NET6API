namespace SoccerPlayerApi.Dtos.Facts;

public class FactCreateDto
{
    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public List<int> DimensionValueIds { get; set; } = new List<int>();
}
