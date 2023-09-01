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

    public virtual async Task CreateAsync(TEntity entity)
    {
        Context.Add(entity);
        await Context.SaveChangesAsync();
    }

    public virtual IQueryable<TEntity> Get()
    {
        return Context
            .Set<TEntity>()
            .AsNoTracking();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await Get()
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(TEntity entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }
}