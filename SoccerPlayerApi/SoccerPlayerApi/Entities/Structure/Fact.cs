using Microsoft.EntityFrameworkCore;

namespace SoccerPlayerApi.Entities.Structure;

public class Fact
{
    public int Id { get; set; }

    public string Type { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public List<DimensionFact> DimensionValues { get; set; }

    //public int CategoryId { get; set; }
    //public DimensionValue Category { get; set; }

    //public int LocationId { get; set; }
    //public DimensionValue Location { get; set; }

    //public int TimeId { get; set; }
    //public DimensionValue Time { get; set; }
}
