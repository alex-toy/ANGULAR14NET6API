using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
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

    [HttpPost("sale")]
    public async Task<int> CreateFact(FactCreateDto fact)
    {
        return await _dimensionService.CreateFactAsync(fact);
    }

    [HttpPost("dimension")]
    public async Task<int> CreateDimension(Dimension dimension)
    {
        return await _dimensionService.CreateDimensionAsync(dimension);
    }

    [HttpPost("level")]
    public async Task<int> CreateLevel(Level dimension)
    {
        return await _dimensionService.CreateLevelAsync(dimension);
    }

    [HttpPost("dimension-value")]
    public async Task<int> CreateDimensionValue(DimensionValue dimensionValue)
    {
        return await _dimensionService.CreateDimensionValueAsync(dimensionValue);
    }

    //[HttpGet("player/{playerId}")]
    //public async Task<Player?> GetById(int playerId)
    //{
    //    Player? articles = await _dimensionService.GetByIdAsync(playerId);
    //    return articles;
    //}
}
