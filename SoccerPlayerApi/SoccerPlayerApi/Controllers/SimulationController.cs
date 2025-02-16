using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Simulations;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Simulations;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SimulationController
{
    private readonly IConfiguration _configuration;
    private readonly ISimulationService _simulationService;

    public SimulationController(IConfiguration configuration, ISimulationService simulationService)
    {
        _configuration = configuration;
        _simulationService = simulationService;
    }

    [HttpGet("algorithms")]
    public async Task<ResponseDto<IEnumerable<AlgorithmDto>>> GetAlgorithms()
    {
        try
        {
            IEnumerable<AlgorithmDto>  algorithms = await _simulationService.GetAlgorithms();
            return new ResponseDto<IEnumerable<AlgorithmDto>> { Data = algorithms, IsSuccess = true, Count = algorithms.Count() };
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<AlgorithmDto>> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
    }
}
