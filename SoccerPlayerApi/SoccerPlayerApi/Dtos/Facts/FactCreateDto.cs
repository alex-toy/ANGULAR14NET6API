namespace SoccerPlayerApi.Dtos.Facts;

public class FactCreateDto
{
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public List<int> AggregationIds { get; set; } = new List<int>();
    public string TimeAggregationLabel { get; set; } = string.Empty;
}
