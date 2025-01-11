using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerPlayerApi.Entities;

namespace SoccerPlayerApi.Repo.Generics;

public class GenericRepo<T> : IGenericRepo<T> where T : Entity
{
    private readonly ApplicationDbContext _context;

    public GenericRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(T player)
    {
        EntityEntry<T> entityPlayer = await _context.Set<T>().AddAsync(player);
        await _context.SaveChangesAsync();
        return entityPlayer.Entity.Id;
    }

    public Task<int> DeleteAsync(int playerId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate)
    {
        List<T> entities = await _context.Set<T>().Where(x => true).ToListAsync();
        return entities;
    }

    public async Task<T?> GetByIdAsync(int playerId)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(p => p.Id == playerId);
    }

    public Task<int> UpdateAsync(T player)
    {
        throw new NotImplementedException();
    }
}
