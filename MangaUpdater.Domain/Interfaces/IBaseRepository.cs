using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    void Create(TEntity entity);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetByIdAsync(int id);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task SaveAsync();
}