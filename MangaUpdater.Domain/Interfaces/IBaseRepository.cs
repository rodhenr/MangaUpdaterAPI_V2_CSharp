using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task CreateAsync(TEntity entity);
    IQueryable<TEntity> Get();
    Task<TEntity?> GetByIdAsync(int id);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
}