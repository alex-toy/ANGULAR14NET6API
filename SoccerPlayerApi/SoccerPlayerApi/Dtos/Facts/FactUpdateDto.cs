namespace SoccerPlayerApi.Dtos.Facts;

public class FactUpdateDto
{
    public int FactId { get; set; }
    public int? DataTypeId { get; set; }
    public decimal? Amount { get; set; }
}
