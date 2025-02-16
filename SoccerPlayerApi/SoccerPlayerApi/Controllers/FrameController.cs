using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Dtos.Frames;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Frames;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FrameController
{
    private readonly IFrameService _frameService;

    public FrameController(IFrameService service)
    {
        _frameService = service;
    }

    [HttpPost("frames")]
    public async Task<ResponseDto<IEnumerable<FrameDto>>> GetFrames()
    {
        IEnumerable<FrameDto> frames = await _frameService.GetFrames();
        return new ResponseDto<IEnumerable<FrameDto>> { Data = frames, IsSuccess = true, Count = frames.Count() };
    }

    [HttpPost("create")]
    public async Task<ResponseDto<int>> CreateFrame(FrameCreateDto frame)
    {
        try
        {
            int frameId = await _frameService.CreateFrame(frame);
            return new ResponseDto<int> { Data = frameId, IsSuccess = true, Count = 1 };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
    }

    [HttpPut("update")]
    public async Task<ResponseDto<int>> Update(FrameUpdateDto frame)
    {
        try
        {
            int frameId = await _frameService.UpdateAsync(frame);
            return new ResponseDto<int> { Data = frameId, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseDto<int> { IsSuccess = false, Count = 0, Message = ex.Message };
        }
    }

    [HttpGet("frame/{id}")]
    public async Task<ResponseDto<FrameDto?>> GetFrameById(int id)
    {
        FrameDto? frame = await _frameService.GetFrameById(id);
        return new ResponseDto<FrameDto?> 
        { 
            Data = frame ?? null, 
            IsSuccess = frame is not null, 
            Count = frame is not null ? 1 : 0
        };
    }

    [HttpDelete("delete/{id}")]
    public async Task<ResponseDto<bool>> DeleteFrame(int id)
    {
        bool result = await _frameService.DeleteById(id);
        return new ResponseDto<bool> { IsSuccess = result };
    }
}
