namespace SoccerPlayerApi.Entities.Structure;

public class TimeLevel
{
    public int Id { get; set; }

    public string Value { get; set; }


    public int? AncestorId { get; set; }
    public TimeLevel? Ancestor { get; set; }
}
