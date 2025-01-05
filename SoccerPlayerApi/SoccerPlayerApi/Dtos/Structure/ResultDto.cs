namespace SoccerPlayerApi.Dtos.Structure;

public abstract class ResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
