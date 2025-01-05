using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Players;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FactController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;

    public FactController(IConfiguration configuration, IPlayerService playerService, IDimensionService dimensionService)
    {
        _configuration = configuration;
        _dimensionService = dimensionService;
    }

    [HttpPost("facts")]
    public async Task<GetFactsResultDto> GetFacts(GetFactFilterDto filter)
    {
        IEnumerable<GetFactResultDto> facts = await _dimensionService.GetFacts(filter);
        return new GetFactsResultDto { Facts = facts, IsSuccess = true };
    }

    [HttpPost("createfact")]
    public async Task<FactCreateResultDto> CreateFact(FactCreateDto fact)
    {
        return await _dimensionService.CreateFactAsync(fact);
    }
}
