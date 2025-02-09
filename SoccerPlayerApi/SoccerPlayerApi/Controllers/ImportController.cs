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
            IEnumerable<ImportErrorDto> results = await _importService.CreateImportFactAsync(facts);
            var importResult = new ImportFactCreateResultDto()
            {
                LinesCreatedCount = 1111,
                ImportErrors = results.ToList(),
                Message = results.Count() > 0 ? "errors occured" : "import ok"
            };
            return new ResponseDto<ImportFactCreateResultDto> { Data = importResult, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<ImportFactCreateResultDto> { IsSuccess = false, Message = ex.Message };
        }
    }
}
