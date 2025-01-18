using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Players;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DimensionController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;

    public DimensionController(IConfiguration configuration, IPlayerService playerService, IDimensionService dimensionService)
    {
        _configuration = configuration;
        _dimensionService = dimensionService;
    }

    [HttpGet("dimensions")]
    public async Task<ResponseDto<IEnumerable<DimensionDto>>> GetDimensions()
    {
        IEnumerable<DimensionDto> dimensions = await _dimensionService.GetDimensions();
        return new ResponseDto<IEnumerable<DimensionDto>> { Data = dimensions, IsSuccess = true };
    }

    [HttpGet("dimensioncount")]
    public async Task<ResponseDto<int>> GetDimensionCount()
    {
        int dimensionCount = await _dimensionService.GetDimensionCount();
        return new ResponseDto<int> { Data = dimensionCount, IsSuccess = true };
    }

    [HttpPost("dimension")]
    public async Task<int> CreateDimension(Dimension dimension)
    {
        return await _dimensionService.CreateDimensionAsync(dimension);
    }

    [HttpGet("dimensionvalues/{levelId}")]
    public async Task<GetAggregationsResultDto> GetDimensionValues(int levelId)
    {
        IEnumerable<GetAggregationDto> dimensionValues = await _dimensionService.GetDimensionValues(levelId);
        return new GetAggregationsResultDto { Aggregations = dimensionValues, IsSuccess = true };
    }

    [HttpPost("aggregation")]
    public async Task<int> CreateDimensionValue(AggregationCreateDto dimensionValue)
    {
        return await _dimensionService.CreateDimensionValueAsync(dimensionValue);
    }
}
