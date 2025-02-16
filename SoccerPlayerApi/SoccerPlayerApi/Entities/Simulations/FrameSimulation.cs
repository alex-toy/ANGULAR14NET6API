using SoccerPlayerApi.Entities.Frames;
using SoccerPlayerApi.Entities.Simulations.Algorithms;

namespace SoccerPlayerApi.Entities.Simulations;

public class FrameSimulation : Entity
{
    public int FrameId { get; set; }
    public Frame Frame { get; set; }

    public int AlgorithmId { get; set; }
    public Algorithm Algorithm { get; set; }
    public List<AlgorithmParameterValue> Values { get; set; }

    public List<SimulationFact> SimulationFacts { get; set; }
}
