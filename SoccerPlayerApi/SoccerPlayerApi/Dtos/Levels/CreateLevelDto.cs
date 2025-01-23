namespace SoccerPlayerApi.Dtos.Levels;

public class CreateLevelDto
{
    public string Value { get; set; }
    public int DimensionId { get; set; }
    public int? AncestorId { get; set; }
}
