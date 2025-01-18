using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities;

public class Environment : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int LevelIdFilter1 { get; set; }
    public Level? LevelFilter1 { get; set; } = new Level();

    public int? LevelIdFilter2 { get; set; }
    public Level? LevelFilter2 { get; set; } = new Level();

    public int? LevelIdFilter3 { get; set; }
    public Level? LevelFilter3 { get; set; } = new Level();

    public int? LevelIdFilter4 { get; set; }
    public Level? LevelFilter4 { get; set; } = new Level();

    public int? LevelIdFilter5 { get; set; }
    public Level? LevelFilter5 { get; set; } = new Level();

    public EnvironmentDto ToDto()
    {
        return new EnvironmentDto
        {
            Id = Id,
            Name = Name,
            Description = Description,

            LevelIdFilter1 = LevelIdFilter1,
            LevelLabel1 = LevelFilter1?.Value ?? string.Empty,

            LevelIdFilter2 = LevelIdFilter2,
            LevelLabel2 = LevelFilter2?.Value ?? string.Empty,

            LevelIdFilter3 = LevelIdFilter3,
            LevelLabel3 = LevelFilter3?.Value ?? string.Empty,

            LevelIdFilter4 = LevelIdFilter4,
            LevelLabel4 = LevelFilter4?.Value ?? string.Empty,

            LevelIdFilter5 = LevelIdFilter5,
            LevelLabel5 = LevelFilter5?.Value ?? string.Empty,
        };
    }
}
