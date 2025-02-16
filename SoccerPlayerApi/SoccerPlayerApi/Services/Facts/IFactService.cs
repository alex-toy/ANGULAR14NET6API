using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Imports;
using SoccerPlayerApi.Dtos.Scopes;

namespace SoccerPlayerApi.Services.Facts;

public interface IFactService
{
    Task<int> CreateFactAsync(FactCreateDto fact);
    Task<DataTypeDto> CreateTypeAsync(DataTypeCreateDto fact);
    Task<IEnumerable<FactDto>> GetFacts(GetFactFilterDto filter);
    Task<IEnumerable<DataTypeDto>> GetFactTypes();
    Task<IEnumerable<GetScopeDataDto>> GetScopeData(FrameScopeDto scope);
    Task<IEnumerable<FrameScopeDto>> GetScopes(ScopeFilterDto filter);
    Task<IEnumerable<FrameScopeDto>> GetScopesByFrameId(int universeId);
    Task<IEnumerable<TimeAggregationDto>> GetTimeAggregations(int levelId);
    Task<IEnumerable<DataTypeDto>> GetDataTypes();
    Task<bool> UpdateFactAsync(FactUpdateDto fact);
    Task<Dictionary<int, List<GetScopeDataDto>>> GetScopeDataTest(FrameScopeDto scope);
}