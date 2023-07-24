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

    public async Task<Manga> CreateAsync(Manga manga)
    {
        _context.Add(manga);
        await _context.SaveChangesAsync();
        return manga;
    }

    public async Task<Manga> GetByIdAsync(int id)
    {
        var mangas = await _context.Mangas.FindAsync(id);
        return mangas;
    }

    public async Task<IEnumerable<Manga>> GetAsync()
    {
        var mangas = await _context.Mangas.ToListAsync();
        return mangas;
    }
}
