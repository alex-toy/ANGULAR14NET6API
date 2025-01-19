using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Dtos.Settings;
using SoccerPlayerApi.Dtos.Structure;
using SoccerPlayerApi.Services.Settings;

namespace SoccerPlayerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController
{
    private readonly IConfiguration _configuration;
    private readonly ISettingsService _settingsService;

    public SettingsController(IConfiguration configuration, ISettingsService service)
    {
        _configuration = configuration;
        _settingsService = service;
    }

    [HttpGet("settings")]
    public async Task<ResponseDto<IEnumerable<SettingsDto>>> GetSettings()
    {
        IEnumerable<SettingsDto> settings = await _settingsService.GetSettings();
        return new ResponseDto<IEnumerable<SettingsDto>> { Data = settings, IsSuccess = true };
    }

    [HttpPut("update")]
    public async Task<ResponseDto<bool>> UpdateSetting(SettingsDto settingsDto)
    {
        bool settingId = await _settingsService.UpdateSettings(settingsDto);
        return new ResponseDto<bool> { Data = settingId, IsSuccess = true };
    }
}

