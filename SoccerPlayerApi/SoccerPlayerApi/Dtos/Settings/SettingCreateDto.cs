using SoccerPlayerApi.Entities;

namespace SoccerPlayerApi.Dtos.Settings;

public class SettingCreateDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public Setting ToDb()
    {
        return new Setting { Key = Key, Value = Value };
    }
}
