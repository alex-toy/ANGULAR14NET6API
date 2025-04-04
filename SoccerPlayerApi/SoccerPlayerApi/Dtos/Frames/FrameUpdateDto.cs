﻿using SoccerPlayerApi.Entities.Frames;

namespace SoccerPlayerApi.Dtos.Frames;

public class FrameUpdateDto
{
    public int Id { get; set; }
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

    public List<FrameSortingDto> FrameSortings { get; set; } = new List<FrameSortingDto>();

    public FrameCreateDto ToCeateDto()
    {
        return new FrameCreateDto
        {
            Name = Name,
            Description = Description,

            Dimension1Id = Dimension1Id,
            LevelIdFilter1 = LevelIdFilter1,

            Dimension2Id = Dimension2Id,
            LevelIdFilter2 = LevelIdFilter2,

            Dimension3Id = Dimension3Id,
            LevelIdFilter3 = LevelIdFilter3,

            Dimension4Id = Dimension4Id,
            LevelIdFilter4 = LevelIdFilter4,
        };
    }

    public Frame ToDb()
    {
        return new Frame
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
