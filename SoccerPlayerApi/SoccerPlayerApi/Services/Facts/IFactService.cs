using SoccerPlayerApi.Dtos.Facts;

namespace SoccerPlayerApi.Services.Facts
{
    public interface IFactService
    {
        Task<FactCreateResultDto> CreateFactAsync(FactCreateDto fact);
        Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactFilterDto filter);
        Task<IEnumerable<string>> GetFactTypes();
        Task<bool> UpdateFactAsync(FactUpdateDto fact);
    }
}