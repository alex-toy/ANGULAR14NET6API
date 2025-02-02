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
    public async Task<ResponseDto<int>> CreateFact(FactCreateDto fact)
    {
        try
        {
            int factId = await _factService.CreateFactAsync(fact);
            return new ResponseDto<int> { Data = factId, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, Message = ex.Message };
        }
    }

    [HttpPost("updatefact")]
    public async Task<bool> UpdateFact(FactUpdateDto fact)
    {
        return await _factService.UpdateFactAsync(fact);
    }

    [HttpGet("facttypes")]
    public async Task<ResponseDto<IEnumerable<DataTypeDto>>> GetFactTypes() // a modifier pour qu'elle ne reamène que les types liés à un scope
    {
        IEnumerable<DataTypeDto> levels = await _factService.GetFactTypes();
        return new ResponseDto<IEnumerable<DataTypeDto>> { Data = levels, IsSuccess = true };
    }

    [HttpGet("types")]
    public async Task<ResponseDto<IEnumerable<DataTypeDto>>> GetDataTypes()
    {
        IEnumerable<DataTypeDto> levels = await _factService.GetDataTypes();
        return new ResponseDto<IEnumerable<DataTypeDto>> { Data = levels, IsSuccess = true };
    }

    [HttpPost("createtype")]
    public async Task<ResponseDto<DataTypeDto>> CreateType(TypeCreateDto fact)
    {
        DataTypeDto type = await _factService.CreateTypeAsync(fact);
        return new ResponseDto<DataTypeDto> { Data = type, IsSuccess = true };
    }
}
