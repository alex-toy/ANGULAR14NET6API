using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("levelstest/{dimensionId}")]
    public async Task<GetLevelsResultDto> GetLevelsTest(int dimensionId)
    {
        IEnumerable<GetLevelDto> levels = await _levelService.GetLevels(dimensionId);
        return new GetLevelsResultDto { Levels = levels, IsSuccess = true };
    }

    [HttpPost("level")]
    public async Task<int> CreateLevel(CreateLevelDto dimension)
    {
        return await _levelService.CreateLevelAsync(dimension);
    }
}
