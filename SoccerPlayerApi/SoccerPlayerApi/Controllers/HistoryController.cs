﻿using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Facts;
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

    [HttpGet("timeaggregations/{levelId}")]
    public async Task<ResponseDto<IEnumerable<TimeAggregationDto>>> GetTimeAggregations(int levelId)
    {
        IEnumerable<TimeAggregationDto> timeAggregations = await _factService.GetTimeAggregations(levelId);
        return new ResponseDto<IEnumerable<TimeAggregationDto>> { Data = timeAggregations, IsSuccess = true, Count = timeAggregations.Count() };
    }
}
