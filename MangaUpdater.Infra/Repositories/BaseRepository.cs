using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
{
    protected readonly IdentityMangaUpdaterContext Context;

    protected BaseRepository(IdentityMangaUpdaterContext context)
    {
        Context = context;
    }

    public virtual void Create(TEntity entity)
    {
        Context.Add(entity);
    }

    protected IQueryable<TEntity> Get()
    {
        return Context
            .Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAsync()
    {
        return await Get()
            .ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await Get()
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public virtual void Update(TEntity entity)
    {
        Context.Update(entity);
    }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }
}