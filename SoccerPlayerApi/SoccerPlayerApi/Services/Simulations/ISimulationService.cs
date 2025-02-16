using SoccerPlayerApi.Dtos.Simulations;

namespace SoccerPlayerApi.Services.Simulations
{
    public interface ISimulationService
    {
        Task<IEnumerable<AlgorithmDto>> GetAlgorithms();
    }
}