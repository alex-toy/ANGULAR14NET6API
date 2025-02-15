using SoccerPlayerApi.Entities.Forecasts.Algorithms;

namespace SoccerPlayerApi.Entities.Forecasts;

public class Simulation : Entity
{
    public int EnvironmentScopeId { get; set; }

    public int AlgorithmId { get; set; }
    public Algorithm Algorithm { get; set; }
    public List<AlgorithmParameterValue> Values { get; set; }

    public List<SimulationFact> SimulationFacts { get; set; }
}
