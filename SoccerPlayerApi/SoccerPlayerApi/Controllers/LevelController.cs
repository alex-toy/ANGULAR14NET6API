using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Levels;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LevelController
{
    private readonly IConfiguration _configuration;
    private readonly ILevelService _levelService;

    public LevelController(IConfiguration configuration, ILevelService dimensionService)
    {
        _configuration = configuration;
        _levelService = dimensionService;
    }

    [HttpGet("levels/{dimensionId}")]
    public async Task<ResponseDto<IEnumerable<GetLevelDto>>> GetLevels(int dimensionId)
    {
        IEnumerable<GetLevelDto> levels = await _levelService.GetLevels(dimensionId);
        return new ResponseDto<IEnumerable<GetLevelDto>> { Data = levels, IsSuccess = true };
    }

    [HttpGet("dimensionlevels")]
    public async Task<ResponseDto<IEnumerable<GetDimensionLevelDto>>> GetDimensionLevels()
    {
        IEnumerable<GetDimensionLevelDto> dimensionLevels = await _levelService.GetDimensionLevels();
        return new ResponseDto<IEnumerable<GetDimensionLevelDto>> { Data = dimensionLevels, IsSuccess = true };
    }

    [HttpPost("level")]
    public async Task<ResponseDto<int>> CreateLevel(CreateLevelDto dimension)
    {
        int id = await _levelService.CreateLevelAsync(dimension);
        return new ResponseDto<int> { Data = id, IsSuccess = true };
    }
}
