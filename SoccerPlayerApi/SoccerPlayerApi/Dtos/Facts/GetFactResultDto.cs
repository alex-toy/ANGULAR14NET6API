namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactResultDto
{
    public int FactId { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public List<DimensionResultDto> Dimensions { get; set; }
}
