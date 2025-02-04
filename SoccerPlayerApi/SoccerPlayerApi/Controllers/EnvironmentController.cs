using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Environment;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Environments;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentController
{
    private readonly IEnvironmentService _environmentService;

    public EnvironmentController(IEnvironmentService service)
    {
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
            int environmentId = await _environmentService.CreateEnvironment(environment);
            return new ResponseDto<int> { Data = environmentId, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
    }

    [HttpPut("update")]
    public async Task<ResponseDto<int>> Update(EnvironmentUpdateDto environment)
    {
        try
        {
            int playerId = await _environmentService.UpdateAsync(environment);
            return new ResponseDto<int> { Data = playerId, IsSuccess = true };
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

    [HttpDelete("delete/{id}")]
    public async Task<ResponseDto<bool>> DeleteEnvironment(int id)
    {
        bool result = await _environmentService.DeleteById(id);
        return new ResponseDto<bool> { IsSuccess = result };
    }
}
