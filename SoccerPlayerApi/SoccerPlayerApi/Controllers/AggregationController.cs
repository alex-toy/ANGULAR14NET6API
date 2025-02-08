using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Aggregations;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Aggregations;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AggregationController
{
    private readonly IConfiguration _configuration;
    private readonly IAggregationService _aggregationService;

    public AggregationController(IConfiguration configuration, IAggregationService service)
    {
        _configuration = configuration;
        _aggregationService = service;
    }

    [HttpPost("aggregation")]
    public async Task<ResponseDto<int>> CreateAggregation(AggregationCreateDto dimensionValue)
    {
        int temp = await _aggregationService.CreateAggregation(dimensionValue);
        return new ResponseDto<int> { Data = temp, IsSuccess = true };
    }

    [HttpGet("aggregations")]
    public async Task<ResponseDto<IEnumerable<GetAggregationDto>>> GetAggregations()
    {
        IEnumerable<GetAggregationDto> aggregations = await _aggregationService.GetAggregations();
        return new ResponseDto<IEnumerable<GetAggregationDto>> { Data = aggregations, IsSuccess = true };
    }

    [HttpGet("motheraggregations/{levelId}")]
    public async Task<ResponseDto<IEnumerable<GetAggregationDto>>> GetMotherAggregations(int levelId)
    {
        IEnumerable<GetAggregationDto> aggregations = await _aggregationService.GetMotherAggregations(levelId);
        return new ResponseDto<IEnumerable<GetAggregationDto>> { Data = aggregations, IsSuccess = true };
    }
}
