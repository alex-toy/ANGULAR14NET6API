using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Scopes;

namespace SoccerPlayerApi.Services.Facts
{
    public interface IFactService
    {
        Task<FactCreateResultDto> CreateFactAsync(FactCreateDto fact);
        Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactFilterDto filter);
        Task<IEnumerable<string>> GetFactTypes();
        Task<IEnumerable<ScopeDto>> GetScopes(ScopeFilterDto? filter);
        Task<bool> UpdateFactAsync(FactUpdateDto fact);
    }
}