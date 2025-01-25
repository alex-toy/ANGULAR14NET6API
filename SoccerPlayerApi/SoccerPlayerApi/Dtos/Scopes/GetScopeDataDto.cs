namespace SoccerPlayerApi.Dtos.Scopes;

public class GetScopeDataDto
{
    public int FactId { get; set; }
    public int TypeId { get; set; }
    public string TypeLabel { get; set; }
    public decimal Amount { get; set; }
    public TimeDimensionDto TimeDimension { get; set; }
    public List<int> AggregationIds { get; set; } = new List<int>();
}
