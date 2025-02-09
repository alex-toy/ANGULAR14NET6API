namespace SoccerPlayerApi.Dtos.Imports;

public class ImportFactCreateResultDto
{
    public int LinesCreatedCount { get; set; }
    public string Message { get; set; }
    public List<ImportErrorDto> ImportErrors { get; set; }
}
