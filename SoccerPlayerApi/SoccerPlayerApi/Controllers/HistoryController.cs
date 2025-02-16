using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Dtos.Scopes;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Facts;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HistoryController
{
    private readonly IConfiguration _configuration;
    private readonly IFactService _factService;

    public HistoryController(IConfiguration configuration, IFactService factService)
    {
        _configuration = configuration;
        _factService = factService;
    }

    [HttpPost("scopes")]
    public async Task<ResponseDto<IEnumerable<FrameScopeDto>>> GetScopes(ScopeFilterDto? filter)
    {
        IEnumerable<FrameScopeDto> scopes = await _factService.GetScopes(filter);
        return new ResponseDto<IEnumerable<FrameScopeDto>> { Data = scopes, IsSuccess = true };
    }

    [HttpGet("scopesbyframeid/{frameId}")]
    public async Task<ResponseDto<IEnumerable<FrameScopeDto>>> GetScopesByFrameId(int frameId)
    {
        IEnumerable<FrameScopeDto> scopes = await _factService.GetScopesByFrameId(frameId);
        return new ResponseDto<IEnumerable<FrameScopeDto>> { Data = scopes, IsSuccess = true };
    }

    [HttpPost("data")]
    public async Task<ResponseDto<IEnumerable<GetScopeDataDto>>> GetScopeData(FrameScopeDto scope)
    {
        IEnumerable<GetScopeDataDto> facts = await _factService.GetScopeData(scope);
        return new ResponseDto<IEnumerable<GetScopeDataDto>> { Data = facts, IsSuccess = true };
    }

    [HttpPost("scopedata")]
    public async Task<ResponseDto<Dictionary<int, List<GetScopeDataDto>>>> GetScopeDataTest(FrameScopeDto scope)
    {
        try
        {
            Dictionary<int, List<GetScopeDataDto>> facts = await _factService.GetScopeDataTest(scope);
            return new ResponseDto<Dictionary<int, List<GetScopeDataDto>>> { Data = facts, IsSuccess = true, Count = facts.Count };
        }
        catch (Exception ex)
        {
            return new ResponseDto<Dictionary<int, List<GetScopeDataDto>>> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
    }

    [HttpGet("timeaggregations/{levelId}")]
    public async Task<ResponseDto<IEnumerable<TimeAggregationDto>>> GetTimeAggregations(int levelId)
    {
        IEnumerable<TimeAggregationDto> timeAggregations = await _factService.GetTimeAggregations(levelId);
        return new ResponseDto<IEnumerable<TimeAggregationDto>> { Data = timeAggregations, IsSuccess = true, Count = timeAggregations.Count() };
    }
}
