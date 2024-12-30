using SoccerPlayerApi.Entities;

namespace SoccerPlayerApi.Repo;

public interface IGenericService<T>
{
    Task<T?> GetByIdAsync(int playerId);
    Task<IEnumerable<T>> GetAllAsync();
    Task<int> CreateAsync(Player player);
    Task<int> UpdateAsync(Player player);
    Task<int> DeleteAsync(int playerId);
}