namespace SoccerPlayerApi.Dtos.Levels;

public class GetLevelDto
{
    public int Id { get; set; }
    public string Value { get; set; }
    public int DimensionId { get; set; }
    public int? AncestorId { get; set; }
}
