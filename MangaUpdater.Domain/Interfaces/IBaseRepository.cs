using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    void CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetByIdAsync(int id);
    void UpdateAsync(TEntity entity);
    void RemoveAsync(TEntity entity);
    Task SaveAsync();
}