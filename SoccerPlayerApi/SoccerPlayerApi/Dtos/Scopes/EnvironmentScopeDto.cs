namespace SoccerPlayerApi.Dtos.Scopes;

public class EnvironmentScopeDto
{
    public int EnvironmentId { get; set; }

    public int Dimension1Id { get; set; }
    public int Dimension1AggregationId { get; set; }
    public string? Dimension1AggregationLabel { get; set; }
    public string? Level1Label { get; set; }
    public string? Dimension1Label { get; set; }

    public int? Dimension2Id { get; set; }
    public int? Dimension2AggregationId { get; set; }
    public string? Dimension2AggregationLabel { get; set; }
    public string? Level2Label { get; set; }
    public string? Dimension2Label { get; set; }

    public int? Dimension3Id { get; set; }
    public int? Dimension3AggregationId { get; set; }
    public string? Dimension3AggregationLabel { get; set; }
    public string? Level3Label { get; set; }
    public string? Dimension3Label { get; set; }

    public int? Dimension4Id { get; set; }
    public int? Dimension4AggregationId { get; set; }
    public string? Dimension4AggregationLabel { get; set; }
    public string? Level4Label { get; set; }
    public string? Dimension4Label { get; set; }

    public string? SortingValue { get; set; }
}
