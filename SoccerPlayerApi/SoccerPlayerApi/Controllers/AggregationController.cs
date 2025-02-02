using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Aggregations;

namespace SoccerPlayerApi.Controllers;

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
    public async Task<int> CreateAggregation(AggregationCreateDto dimensionValue)
    {
        return await _aggregationService.CreateAggregation(dimensionValue);
    }

    [HttpGet("aggregations")]
    public async Task<ResponseDto<IEnumerable<GetAggregationDto>>> GetAggregations()
    {
        IEnumerable<GetAggregationDto> dimensions = await _aggregationService.GetAggregations();
        return new ResponseDto<IEnumerable<GetAggregationDto>> { Data = dimensions, IsSuccess = true };
    }

    [HttpGet("motheraggregations")]
    public async Task<ResponseDto<IEnumerable<GetAggregationDto>>> GetMotherAggregations()
    {
        string query = "SELECT AGG.* \r\n  FROM Aggregations AGG\r\n  JOIN Levels LV ON LV.AncestorId = AGG.LevelId\r\n  where LV.Id = 7";
        IEnumerable<GetAggregationDto> dimensions = await _aggregationService.GetAggregations();
        return new ResponseDto<IEnumerable<GetAggregationDto>> { Data = dimensions, IsSuccess = true };
    }
}
