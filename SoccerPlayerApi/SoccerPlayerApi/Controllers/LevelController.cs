using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Dimensions;
using SoccerPlayerApi.Dtos.Levels;
using SoccerPlayerApi.Services.Levels;
using SoccerPlayerApi.Services.Players;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LevelController
{
    private readonly IConfiguration _configuration;
    private readonly ILevelService _levelService;

    public LevelController(IConfiguration configuration, IPlayerService playerService, ILevelService dimensionService)
    {
        _configuration = configuration;
        _levelService = dimensionService;
    }

    [HttpGet("levels/{dimensionId}")]
    public async Task<GetLevelsResultDto> GetLevels(int dimensionId)
    {
        IEnumerable<GetLevelDto> levels = await _levelService.GetLevels(dimensionId);
        return new GetLevelsResultDto { Levels = levels, IsSuccess = true };
    }

    [HttpGet("dimensionlevels")]
    public async Task<GetDimensionLevelsResultDto> GetDimensionLevels()
    {
        IEnumerable<GetDimensionLevelDto> dimensionLevels = await _levelService.GetDimensionLevels();
        return new GetDimensionLevelsResultDto { DimensionLevels = dimensionLevels, IsSuccess = true };
    }

    [HttpPost("level")]
    public async Task<int> CreateLevel(CreateLevelDto dimension)
    {
        return await _levelService.CreateLevelAsync(dimension);
    }
}
