namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactResultDto
{
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public List<DimensionResultDto> Dimensions { get; set; }
}
