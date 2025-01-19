using SoccerPlayerApi.Dtos.Settings;

namespace SoccerPlayerApi.Services.Settings
{
    public interface ISettingsService
    {
        Task<IEnumerable<SettingsDto>> GetSettings();
        Task<bool> UpdateSettings(SettingsDto settingsDto);
    }
}