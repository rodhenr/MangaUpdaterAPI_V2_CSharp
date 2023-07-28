using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;

namespace MangaUpdater.Infra.Data.Repositories;
public class MangaRepository : IMangaRepository
{
    private readonly MangaUpdaterContext _context;

    public MangaRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Manga manga)
    {
        _context.Add(manga);
        await _context.SaveChangesAsync();
        return;
    }

    public async Task<Manga?> GetByIdAsync(int id)
    {
        return await _context.Mangas
            .Include(a => a.MangaGenres)
                .ThenInclude(a => a.Genre)
            .Include(a => a.MangaSources)
                .ThenInclude(a => a.Source)
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Manga>> GetMangasAsync()
    {
        return await _context.Mangas            
            .ToListAsync();
    }
}
