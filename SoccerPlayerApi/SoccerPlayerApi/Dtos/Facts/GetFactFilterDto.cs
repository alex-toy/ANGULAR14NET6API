﻿namespace SoccerPlayerApi.Dtos.Facts;

public class GetFactFilterDto
{
    public string Type { get; set; }
    public List<GetFactDimensionFilterDto> FactDimensionFilters { get; set; } = new List<GetFactDimensionFilterDto>();
    public List<int> AggregationIds { get; set; } = new List<int>();
}
