using CleanArchMvc.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<Manga>> GetMangasAsync()
    {
        var mangas = await _context.Mangas.ToListAsync();
        return mangas;
    }

    public async Task<IEnumerable<Manga>> GetListByIdAsync(IEnumerable<int> ids)
    {
        List<Manga> mangaList = new();

        foreach (var id in ids)
        {
            var manga = await _context.Mangas.FindAsync(id);

            if (manga is not null)
            {
                mangaList.Add(manga);
            }
        }

        return mangaList;
    }
}
