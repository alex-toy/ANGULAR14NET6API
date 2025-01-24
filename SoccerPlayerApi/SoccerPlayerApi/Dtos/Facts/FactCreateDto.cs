﻿namespace SoccerPlayerApi.Dtos.Facts;

public class FactCreateDto
{
    public decimal Amount { get; set; }
    public int DataTypeId { get; set; }
    public List<int> AggregationIds { get; set; } = new List<int>();
    public int TimeAggregationId { get; set; }
}
