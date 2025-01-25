using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Facts;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FactController
{
    private readonly IConfiguration _configuration;
    private readonly IDimensionService _dimensionService;
    private readonly IFactService _factService;

    public FactController(IConfiguration configuration, IDimensionService dimensionService, IFactService factService)
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
    public async Task<ResponseDto<IEnumerable<TypeDto>>> GetFactTypes() // a modifier pour qu'elle ne reamène que les types liés à un scope
    {
        IEnumerable<TypeDto> levels = await _factService.GetFactTypes();
        return new ResponseDto<IEnumerable<TypeDto>> { Data = levels, IsSuccess = true };
    }

    [HttpGet("types")]
    public async Task<ResponseDto<IEnumerable<TypeDto>>> GetTypes()
    {
        IEnumerable<TypeDto> levels = await _factService.GetTypes();
        return new ResponseDto<IEnumerable<TypeDto>> { Data = levels, IsSuccess = true };
    }

    [HttpPost("createtype")]
    public async Task<ResponseDto<TypeDto>> CreateType(TypeCreateDto fact)
    {
        TypeDto type = await _factService.CreateTypeAsync(fact);
        return new ResponseDto<TypeDto> { Data = type, IsSuccess = true };
    }
}
