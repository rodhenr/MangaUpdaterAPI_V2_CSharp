using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly IdentityMangaUpdaterContext _context;

    public GenreRepository(IdentityMangaUpdaterContext context)
    {
        _context = context;
    }

    public Task CreateAsync(Genre entity)
    {
        throw new NotImplementedException();
    }

    public Task BulkCreateAsync(IEnumerable<Genre> entities)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _context.Genres
            .AsNoTracking()
            .SingleOrDefaultAsync(g => g.Id == id);
    }

    public Task DeleteAsync(Genre entity)
    {
        throw new NotImplementedException();
    }
}