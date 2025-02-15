using SoccerPlayerApi.Dtos.Levels;

namespace SoccerPlayerApi.Entities.Structure;

public class Level
{
    public int Id { get; set; }

    public string Label { get; set; }

    public int DimensionId { get; set; }
    public Dimension Dimension { get; set; }

    public int? FatherId { get; set; }
    public Level? Father { get; set; }

    public List<Level> Children { get; set; } = new List<Level>();
    public List<Aggregation> DimensionValues { get; set; } = new List<Aggregation>();

    public ICollection<Environment> Environment1s { get; set; }
    public ICollection<Environment> Environment2s { get; set; }
    public ICollection<Environment> Environment3s { get; set; }
    public ICollection<Environment> Environment4s { get; set; }
    public ICollection<Environment> Environment5s { get; set; }

    public GetLevelDto ToDto()
    {
        return new GetLevelDto
        {
            Id = Id,
            Label = Label,
            DimensionId = DimensionId,
            AncestorId = FatherId,
        };
    }
}
