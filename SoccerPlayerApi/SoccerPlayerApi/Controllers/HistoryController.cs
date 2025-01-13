using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Facts;
using SoccerPlayerApi.Services.Players;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HistoryController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;
    private readonly IFactService _factService;

    public HistoryController(IConfiguration configuration, IPlayerService playerService, IDimensionService dimensionService, IFactService factService)
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
}
