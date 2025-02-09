using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Facts;
using SoccerPlayerApi.Dtos.Imports;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Imports;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController
{
    private readonly IConfiguration _configuration;
    private readonly IImportService _importService;

    public ImportController(IConfiguration configuration, IImportService service)
    {
        _configuration = configuration;
        _importService = service;
    }

    [HttpPost("createimportfact")]
    public async Task<ResponseDto<ImportFactCreateResultDto>> CreateImportFact(IEnumerable<ImportFactDto> facts)
    {
        try
        {
            ImportFactCreateResultDto results = await _importService.CreateImportFactAsync(facts);
            return new ResponseDto<ImportFactCreateResultDto> { Data = results, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ImportFactCreateResultDto> { IsSuccess = false, Message = ex.Message };
        }
    }
}
