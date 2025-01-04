namespace SoccerPlayerApi.Dtos.Facts;

public class FactCreateDto
{
    public string Type { get; set; }

    public decimal Amount { get; set; }

    public List<DimensionFactCreateDto> DimensionFacts { get; set; }
}
