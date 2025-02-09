using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Imports;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Entities.Structure;
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
    public async Task<ResponseDto<IEnumerable<FactDto>>> GetFacts(GetFactFilterDto filter)
    {
        try
        {
            IEnumerable<FactDto> facts = await _factService.GetFacts(filter);
            return new ResponseDto<IEnumerable<FactDto>> { Data = facts, IsSuccess = true, Count = facts.Count() };
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<FactDto>> { IsSuccess = false, Message = ex.Message };
        }
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
    public async Task<ResponseDto<DataTypeDto>> CreateType(DataTypeCreateDto fact)
    {
        DataTypeDto type = await _factService.CreateTypeAsync(fact);
        return new ResponseDto<DataTypeDto> { Data = type, IsSuccess = true };
    }
}
