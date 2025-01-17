using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
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
    public async Task<GetDimensionsResultDto> GetDimensions()
    {
        IEnumerable<DimensionDto> dimensions = await _dimensionService.GetDimensions();
        return new GetDimensionsResultDto { Dimensions = dimensions, IsSuccess = true };
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
