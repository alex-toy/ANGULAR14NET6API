﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Dtos.Settings;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;

namespace SoccerPlayerApi.Services.Settings;

public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _context;

    public SettingsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SettingsDto>> GetSettings()
    {
        return await _context.Settings
            .Select(l => new SettingsDto { Id = l.Id, Value = l.Value, Key = l.Key })
            .ToListAsync();
    }

    public async Task<int> CreateAsync(SettingCreateDto setting)
    {
        EntityEntry<Setting> entity = await _context.Settings.AddAsync(setting.ToDb());
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<bool> UpdateSettings(SettingsDto settingsDto)
    {
        Setting? entity = await _context.Settings.FirstOrDefaultAsync(x => x.Id == settingsDto.Id);

        if (entity is null) return false;

        entity.Key = settingsDto.Key;
        entity.Value = settingsDto.Value;

        await _context.SaveChangesAsync();

        return true;
    }
}
