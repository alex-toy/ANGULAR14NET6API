namespace SoccerPlayerApi.Entities.Structure;

public class TimeLevel : Entity
{
    public string Label { get; set; }


    public int? AncestorId { get; set; }
    public TimeLevel? Ancestor { get; set; }
}
