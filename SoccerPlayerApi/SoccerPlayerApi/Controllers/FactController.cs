using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Facts;
using SoccerPlayerApi.Services.Players;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FactController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;
    private readonly IFactService _factService;

    public FactController(IConfiguration configuration, IPlayerService playerService, IDimensionService dimensionService, IFactService factService)
    {
        _configuration = configuration;
        _dimensionService = dimensionService;
        _factService = factService;
    }

    [HttpPost("facts")]
    public async Task<GetFactsResultDto> GetFacts(GetFactFilterDto filter)
    {
        IEnumerable<GetFactResultDto> facts = await _factService.GetFacts(filter);
        return new GetFactsResultDto { Facts = facts, IsSuccess = true };
    }

    [HttpPost("createfact")]
    public async Task<FactCreateResultDto> CreateFact(FactCreateDto fact)
    {
        return await _factService.CreateFactAsync(fact);
    }

    [HttpPost("updatefact")]
    public async Task<bool> UpdateFact(FactUpdateDto fact)
    {
        return await _factService.UpdateFactAsync(fact);
    }

    [HttpGet("facttypes")]
    public async Task<ResponseDto<IEnumerable<string>>> GetFactTypes()
    {
        IEnumerable<string> levels = await _factService.GetFactTypes();
        return new ResponseDto<IEnumerable<string>> { Data = levels, IsSuccess = true };
    }
}
