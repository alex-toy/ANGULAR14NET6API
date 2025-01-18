using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Facts;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HistoryController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;
    private readonly IFactService _factService;

    public HistoryController(IConfiguration configuration, IDimensionService dimensionService, IFactService factService)
    {
        _configuration = configuration;
        _dimensionService = dimensionService;
        _factService = factService;
    }

    [HttpPost("scopes")]
    public async Task<ResponseDto<IEnumerable<ScopeDto>>> GetScopes(ScopeFilterDto? filter)
    {
        IEnumerable<ScopeDto> scopes = await _factService.GetScopes(filter);
        return new ResponseDto<IEnumerable<ScopeDto>> { Data = scopes, IsSuccess = true };
    }

    [HttpGet("scopesbyenvironmentid/{environmentId}")]
    public async Task<ResponseDto<IEnumerable<ScopeDto>>> GetScopesByEnvironmentId(int environmentId)
    {
        IEnumerable<ScopeDto> scopes = await _factService.GetScopesByEnvironmentId(environmentId);
        return new ResponseDto<IEnumerable<ScopeDto>> { Data = scopes, IsSuccess = true };
    }

    [HttpPost("data")]
    public async Task<ResponseDto<IEnumerable<GetScopeDataDto>>> GetScopeData(ScopeDto scope)
    {
        IEnumerable<GetScopeDataDto> facts = await _factService.GetScopeData(scope);
        return new ResponseDto<IEnumerable<GetScopeDataDto>> { Data = facts, IsSuccess = true };
    }
}
