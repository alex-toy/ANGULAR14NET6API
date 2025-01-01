using SoccerPlayerApi.Entities;

namespace SoccerPlayerApi.Repo;

public interface IGenericRepo<T> where T : Entity
{
    Task<T?> GetByIdAsync(int playerId);
    Task<int> CreateAsync(T player);
    Task<int> UpdateAsync(T player);
    Task<int> DeleteAsync(int playerId);
    Task<IEnumerable<T>> GetAllAsync(Func<T, bool> predicate);
}