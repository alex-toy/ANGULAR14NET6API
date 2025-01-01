using SoccerPlayerApi.Entities.Dimensions;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities;

public class Sale
{
    public int Id { get; set; }
    public decimal SalesAmount { get; set; }

    public int CategoryId { get; set; }
    public DimensionValue<Dimension> Category { get; set; }

    public int LocationId { get; set; }
    public DimensionValue<Location> Location { get; set; }

    public int TimeId { get; set; }
    public DimensionValue<Time> Time { get; set; }
}
