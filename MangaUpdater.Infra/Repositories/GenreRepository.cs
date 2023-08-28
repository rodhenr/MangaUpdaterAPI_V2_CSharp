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

    public async Task<IEnumerable<Genre>> GetAsync()
    {
        return await _context.Genres
            .AsNoTracking()
            .ToListAsync();
    }
}