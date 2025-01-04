namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactDto
{
    public string Type { get; set; }
    public List<GetFactDimensionFilterDto> FactDimensionFilters { get; set; }
}
