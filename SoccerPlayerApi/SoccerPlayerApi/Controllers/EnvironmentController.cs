using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Environments;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController
{
    private readonly IConfiguration _configuration;
    private readonly IEnvironmentService _environmentService;

    public EnvironmentController(IConfiguration configuration, IEnvironmentService service)
    {
        _configuration = configuration;
        _environmentService = service;
    }

    [HttpPost("environments")]
    public async Task<ResponseDto<IEnumerable<EnvironmentDto>>> GetEnvironments()
    {
        IEnumerable<EnvironmentDto> environments = await _environmentService.GetEnvironments();
        return new ResponseDto<IEnumerable<EnvironmentDto>> { Data = environments, IsSuccess = true, Count = environments.Count() };
    }

    [HttpGet("environment/{id}")]
    public async Task<ResponseDto<EnvironmentDto?>> GetEnvironmentById(int id)
    {
        EnvironmentDto? environment = await _environmentService.GetEnvironmentById(id);
        return new ResponseDto<EnvironmentDto?> 
        { 
            Data = environment ?? null, 
            IsSuccess = environment is not null, 
            Count = environment is not null ? 1 : 0
        };
    }
}
