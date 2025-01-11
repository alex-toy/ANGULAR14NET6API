using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;

namespace SoccerPlayerApi.Services.Players;

public class PlayerService : IPlayerService
{
    private readonly ApplicationDbContext _context;

    public PlayerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetByIdAsync(int playerId)
    {
        return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
    }

    //Player? IGenericRepo<Player>.GetByIdAsync(int playerId)
    //{
    //    return _context.Players.FirstOrDefault(p => p.Id == playerId);
    //}

    public async Task<IEnumerable<Player>> GetAllAsync(Func<Player, bool> predicate)
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<int> CreateAsync(Player player)
    {
        EntityEntry<Player> entityPlayer = await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();
        return entityPlayer.Entity.Id;
    }

    public async Task<int> UpdateAsync(Player player)
    {
        Player? playerDb = await GetByIdAsync(player.Id);
        if (playerDb is null) return -1;
        playerDb.Name = player.Name;
        playerDb.JerseyNumber = player.JerseyNumber;
        EntityEntry<Player> entityPlayer = _context.Players.Update(playerDb);
        await _context.SaveChangesAsync();
        return entityPlayer.Entity.Id;
    }

    public async Task<int> DeleteAsync(int playerId)
    {
        Player? player = await GetByIdAsync(playerId);
        if (player is null) return -1;
        EntityEntry<Player> entityPlayer = _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return entityPlayer is not null ? entityPlayer.Entity.Id : -1;
    }
}
