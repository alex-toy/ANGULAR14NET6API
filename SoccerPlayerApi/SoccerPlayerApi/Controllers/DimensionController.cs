using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.DimensionValues;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Dimensions;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DimensionController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;

    public DimensionController(IConfiguration configuration, IDimensionService dimensionService)
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

    [HttpPost("create")]
    public async Task<ResponseDto<int>> CreateDimension(DimensionDto dimension)
    {
        int id = await _dimensionService.CreateDimensionAsync(dimension);
        return new ResponseDto<int> { Data = id, IsSuccess = true };
    }

    [HttpGet("dimensionvalues/{levelId}")]
    public async Task<GetAggregationsResultDto> GetDimensionValues(int levelId)
    {
        IEnumerable<GetAggregationDto> dimensionValues = await _dimensionService.GetDimensionValues(levelId);
        return new GetAggregationsResultDto { Aggregations = dimensionValues, IsSuccess = true };
    }
}
