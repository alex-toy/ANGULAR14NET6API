namespace SoccerPlayerApi.Dtos.Environment;

public class EnvironmentUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int LevelIdFilter1 { get; set; }

    public int? LevelIdFilter2 { get; set; }

    public int? LevelIdFilter3 { get; set; }

    public int? LevelIdFilter4 { get; set; }

    public Entities.Environment ToDb()
    {
        return new Entities.Environment
        {
            Name = Name,
            Description = Description,
            LevelIdFilter1 = LevelIdFilter1,
            LevelIdFilter2 = LevelIdFilter2,
            LevelIdFilter3 = LevelIdFilter3,
            LevelIdFilter4 = LevelIdFilter4
        };
    }
}
