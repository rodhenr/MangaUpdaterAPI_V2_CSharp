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

    public async Task CreateAsync(Genre entity)
    {
        await _context.Genres.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Genre>> GetAsync()
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

    public async Task DeleteAsync(Genre entity)
    {
        _context.Genres.Remove(entity);
        await _context.SaveChangesAsync();
    }
}