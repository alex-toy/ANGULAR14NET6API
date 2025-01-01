using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities;

public class Sale
{
    public int Id { get; set; }
    public decimal SalesAmount { get; set; }

    public int CategoryId { get; set; }
    public DimensionValue Category { get; set; }

    public int LocationId { get; set; }
    public DimensionValue Location { get; set; }

    public int TimeId { get; set; }
    public DimensionValue Time { get; set; }
}
