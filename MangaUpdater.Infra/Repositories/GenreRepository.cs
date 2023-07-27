using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MangaUpdaterContext _context;

    public GenreRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }
}
