namespace SoccerPlayerApi.Entities.Simulations.Algorithms;

public class Algorithm : Entity
{
    public string Label { get; set; }
    public List<AlgorithmParameterKey> Keys { get; set; }
}
