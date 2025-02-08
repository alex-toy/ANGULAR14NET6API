namespace SoccerPlayerApi.Dtos.Levels;

public class TimeLevelDto
{
    public int Id { get; set; }

    public string Label { get; set; }


    public int? AncestorId { get; set; }
    public TimeLevelDto? Ancestor { get; set; }
}
