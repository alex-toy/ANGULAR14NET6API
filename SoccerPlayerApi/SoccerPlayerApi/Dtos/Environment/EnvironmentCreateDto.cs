﻿namespace SoccerPlayerApi.Dtos.Environment;

public class EnvironmentCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int Dimension1Id { get; set; }
    public int LevelIdFilter1 { get; set; }

    public int? Dimension2Id { get; set; }
    public int? LevelIdFilter2 { get; set; }

    public int? Dimension3Id { get; set; }
    public int? LevelIdFilter3 { get; set; }

    public int? Dimension4Id { get; set; }
    public int? LevelIdFilter4 { get; set; }

    public List<EnvironmentSortingDto> EnvironmentSortings { get; set; } = new List<EnvironmentSortingDto>();

    public Entities.Environment ToDb()
    {
        return new Entities.Environment
        {
            Name = Name,
            Description = Description,
            LevelIdFilter1 = LevelIdFilter1,
            LevelIdFilter2 = LevelIdFilter2,
            LevelIdFilter3 = LevelIdFilter3,
            LevelIdFilter4 = LevelIdFilter4,
        };
    }
}
