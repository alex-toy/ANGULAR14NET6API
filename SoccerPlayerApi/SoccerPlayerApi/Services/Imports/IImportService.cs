using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Imports;

namespace SoccerPlayerApi.Services.Imports
{
    public interface IImportService
    {
        Task<IEnumerable<ImportErrorDto>> CreateImportFactAsync(IEnumerable<ImportFactDto> facts);
    }
}