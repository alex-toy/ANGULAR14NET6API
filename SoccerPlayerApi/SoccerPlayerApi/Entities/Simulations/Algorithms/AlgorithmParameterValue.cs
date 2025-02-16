namespace SoccerPlayerApi.Entities.Simulations.Algorithms;

public class AlgorithmParameterValue : Entity
{
    public int AlgorithmId { get; set; }
    public string Value { get; set; }
    public int AlgorithmParameterKeyId { get; set; }
}
