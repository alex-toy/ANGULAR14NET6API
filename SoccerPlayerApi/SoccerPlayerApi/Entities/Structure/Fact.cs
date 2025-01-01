using Microsoft.EntityFrameworkCore;

namespace SoccerPlayerApi.Entities.Structure;

public class Fact : Entity
{
    public string Type { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public List<DimensionFact> DimensionFacts { get; set; }
}
