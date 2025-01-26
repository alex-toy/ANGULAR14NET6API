using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Environments;
using SoccerPlayerApi.Services.Facts;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController
{
    private readonly IConfiguration _configuration;
    private readonly IEnvironmentService _environmentService;
    private readonly IFactService _factService;

    public EnvironmentController(IConfiguration configuration, IEnvironmentService service, IFactService factService)
    {
        _configuration = configuration;
        _environmentService = service;
        _factService = factService;
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
            int environmentId = await _environmentService.CreateEnvironment(environment);

            return new ResponseDto<int> { Data = environmentId, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
    }

    [HttpPut("update")]
    public async Task<int> Update(EnvironmentUpdateDto environment)
    {
        int playerId = await _environmentService.UpdateAsync(environment);
        return playerId;
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

    [HttpDelete("delete/{id}")]
    public async Task<ResponseDto<bool>> DeleteEnvironment(int id)
    {
        bool result = await _environmentService.DeleteById(id);
        return new ResponseDto<bool> { IsSuccess = result };
    }
}
