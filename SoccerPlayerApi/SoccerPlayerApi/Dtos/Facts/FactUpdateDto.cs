namespace SoccerPlayerApi.Dtos.Facts;

public class FactUpdateDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
