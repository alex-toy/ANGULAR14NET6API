using SoccerPlayerApi.Entities.Frames;

namespace SoccerPlayerApi.Dtos.Simulations;

public class FrameSimulationCreateDto
{
    public int FrameId { get; set; }
    public Frame Frame { get; set; }

    public int AlgorithmId { get; set; }
    public AlgorithmDto Algorithm { get; set; }
    public List<AlgorithmParameterValueDto> Values { get; set; }
}
