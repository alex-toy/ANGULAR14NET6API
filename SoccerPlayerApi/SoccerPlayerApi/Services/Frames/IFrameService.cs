using SoccerPlayerApi.Dtos.Frames;

namespace SoccerPlayerApi.Services.Frames;

public interface IFrameService
{
    Task<int> CreateFrame(FrameCreateDto environment);
    Task<bool> DeleteById(int id);
    Task<FrameDto?> GetFrameById(int id);
    Task<IEnumerable<FrameDto>> GetFrames();
    Task<int> UpdateAsync(FrameUpdateDto environment);
}