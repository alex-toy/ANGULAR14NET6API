namespace SoccerPlayerApi.Dtos.Environment;

public class EnvironmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int Dimension1Id { get; set; }
    public int LevelIdFilter1 { get; set; }
    public string LevelLabel1 { get; set; } = string.Empty;

    public int? Dimension2Id { get; set; }
    public int? LevelIdFilter2 { get; set; }
    public string LevelLabel2 { get; set; } = string.Empty;

    public int? Dimension3Id { get; set; }
    public int? LevelIdFilter3 { get; set; }
    public string LevelLabel3 { get; set; } = string.Empty;

    public int? Dimension4Id { get; set; }
    public int? LevelIdFilter4 { get; set; }
    public string LevelLabel4 { get; set; } = string.Empty;

    public List<EnvironmentSortingDto> EnvironmentSortings { get; set; } = new List<EnvironmentSortingDto>();
}
