﻿namespace SoccerPlayerApi.Entities.Structure;

public class DimensionValue : Entity
{
    public int LevelId { get; set; }
    public Level Level { get; set; }

    public string Value { get; set; } = string.Empty;
    public DimensionFact DimensionFact { get; set; } = new DimensionFact();
}
