﻿namespace SoccerPlayerApi.Dtos.Levels;

public class CreateLevelDto
{
    public string Label { get; set; }
    public int DimensionId { get; set; }
    public int? AncestorId { get; set; }
}
