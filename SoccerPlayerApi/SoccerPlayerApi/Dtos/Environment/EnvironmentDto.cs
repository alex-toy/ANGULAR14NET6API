namespace SoccerPlayerApi.Dtos.Environment;

public class EnvironmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int LevelIdFilter1 { get; set; }
    public string LevelLabel1 { get; set; } = string.Empty;

    public int? LevelIdFilter2 { get; set; }
    public string LevelLabel2 { get; set; } = string.Empty;

    public int? LevelIdFilter3 { get; set; }
    public string LevelLabel3 { get; set; } = string.Empty;

    public int? LevelIdFilter4 { get; set; }
    public string LevelLabel4 { get; set; } = string.Empty;

    public List<EnvironmentSortingDto> EnvironmentSortings { get; set; } = new List<EnvironmentSortingDto>();
}
