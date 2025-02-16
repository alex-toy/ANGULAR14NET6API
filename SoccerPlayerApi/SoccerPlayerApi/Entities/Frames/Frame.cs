using SoccerPlayerApi.Dtos.Frames;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Entities.Frames;

public class Frame : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int LevelIdFilter1 { get; set; }
    public Level LevelFilter1 { get; set; }

    public int? LevelIdFilter2 { get; set; }
    public Level? LevelFilter2 { get; set; }

    public int? LevelIdFilter3 { get; set; }
    public Level? LevelFilter3 { get; set; }

    public int? LevelIdFilter4 { get; set; }
    public Level? LevelFilter4 { get; set; }

    public List<FrameScope> FrameScopes { get; set; }

    public List<FrameSorting> FrameSortings { get; set; }

    public void Map(FrameUpdateDto frame)
    {
        Name = frame.Name;
        Description = frame.Description;
        LevelIdFilter1 = frame.LevelIdFilter1;
        LevelIdFilter2 = frame.LevelIdFilter2;
        LevelIdFilter3 = frame.LevelIdFilter3;
        LevelIdFilter4 = frame.LevelIdFilter4;
    }

    public FrameDto ToDto() => new FrameDto
    {
        Id = Id,
        Name = Name,
        Description = Description,

        Dimension1Id = LevelFilter1.DimensionId,
        LevelIdFilter1 = LevelIdFilter1,
        LevelLabel1 = LevelFilter1?.Label ?? string.Empty,

        Dimension2Id = LevelFilter2?.DimensionId,
        LevelIdFilter2 = LevelIdFilter2,
        LevelLabel2 = LevelFilter2?.Label ?? string.Empty,

        Dimension3Id = LevelFilter3?.DimensionId,
        LevelIdFilter3 = LevelIdFilter3,
        LevelLabel3 = LevelFilter3?.Label ?? string.Empty,

        Dimension4Id = LevelFilter4?.DimensionId,
        LevelIdFilter4 = LevelIdFilter4,
        LevelLabel4 = LevelFilter4?.Label ?? string.Empty,

        FrameSortings = FrameSortings.Select(x => x.ToDto()).ToList(),
    };
}
