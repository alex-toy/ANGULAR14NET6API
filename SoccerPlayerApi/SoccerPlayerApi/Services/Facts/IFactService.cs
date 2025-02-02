using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Scopes;

namespace SoccerPlayerApi.Services.Facts;

public interface IFactService
{
    Task<int> CreateFactAsync(FactCreateDto fact);
    Task<DataTypeDto> CreateTypeAsync(TypeCreateDto fact);
    Task<IEnumerable<GetFactResultDto>> GetFacts(GetFactFilterDto filter);
    Task<IEnumerable<DataTypeDto>> GetFactTypes();
    Task<IEnumerable<GetScopeDataDto>> GetScopeData(EnvironmentScopeDto scope);
    Task<IEnumerable<EnvironmentScopeDto>> GetScopes(ScopeFilterDto? filter);
    Task<IEnumerable<EnvironmentScopeDto>> GetScopesByEnvironmentId(int universeId);
    Task<IEnumerable<TimeAggregationDto>> GetTimeAggregations(int levelId);
    Task<IEnumerable<DataTypeDto>> GetDataTypes();
    Task<bool> UpdateFactAsync(FactUpdateDto fact);
}