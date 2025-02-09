using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Imports;

namespace SoccerPlayerApi.Services.Imports;

public interface IImportService
{
    Task<ImportFactCreateResultDto> CreateImportFactAsync(IEnumerable<ImportFactDto> facts);
}