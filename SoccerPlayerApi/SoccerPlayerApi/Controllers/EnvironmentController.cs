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

    [HttpPost("create")]
    public async Task<ResponseDto<int>> CreateEnvironment(EnvironmentCreateDto environment)
    {
        try
        {
            int id = await _environmentService.CreateEnvironment(environment);
            return new ResponseDto<int> { Data = id, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
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
