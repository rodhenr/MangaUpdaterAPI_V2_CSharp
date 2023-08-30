namespace MangaUpdater.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task DeleteAsync(TEntity entity);
}