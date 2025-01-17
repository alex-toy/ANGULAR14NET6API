namespace SoccerPlayerApi.Entities;

public class Environment : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DimensionIdFilter1 { get; set; }
    public int LevelIdFilter1 { get; set; }
    public int DimensionIdFilter2 { get; set; }
    public int LevelIdFilter2 { get; set; }
    public int DimensionIdFilter3 { get; set; }
    public int LevelIdFilter3 { get; set; }
    public int DimensionIdFilter4 { get; set; }
    public int LevelIdFilter4 { get; set; }
    public int DimensionIdFilter5 { get; set; }
    public int LevelIdFilter5 { get; set; }
}
