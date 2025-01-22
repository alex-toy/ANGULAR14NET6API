namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactResultDto
{
    public int Id { get; set; }
    public int DataType { get; set; }
    public decimal Amount { get; set; }
    public List<DimensionResultDto> Dimensions { get; set; }
}
