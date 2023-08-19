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
        await _context.AddAsync(manga);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task<IEnumerable<Manga>> GetAsync()
    {
        return await _context.Mangas
            .Include(m => m.MangaSources)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Manga>> GetWithFiltersAsync(string? orderBy, List<int>? sourceIdList, List<int>? genreIdList)
    {
        var query = _context.Mangas
            .AsQueryable();

        if (!string.IsNullOrEmpty(orderBy))
        {
            switch (orderBy.ToLower())
            {
                case "alphabet":
                    query = query
                        .OrderBy(a => a.Name);
                    break;
                case "latest":
                    query = query
                        .OrderByDescending(a => a.Id);
                    break;
                default:
                    break;
            }
        }

        if (sourceIdList != null && sourceIdList.Any())
        {
            query = query.Where(a => a.MangaSources.Any(b => sourceIdList.Contains(b.SourceId)));
        }

        if (genreIdList != null && genreIdList.Any())
        {
            query = query.Where(a => a.MangaGenres.Any(b => genreIdList.Contains(b.GenreId)));
        }

        return await query
            .Include(a => a.MangaSources)
            .Include(a => a.MangaGenres)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Manga?> GetByIdOrderedDescAsync(int id)
    {
        return await _context.Mangas
            .Include(a => a.MangaGenres)
                .ThenInclude(a => a.Genre)
            .Include(a => a.MangaSources)
                .ThenInclude(a => a.Source)
            .Include(a => a.Chapters.OrderByDescending(b => b.Date))
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, int userId)
    {
        return await _context.Mangas
            .Include(a => a.UserMangas.Where(b => b.UserId == userId))
            .Include(a => a.MangaGenres)
                .ThenInclude(a => a.Genre)
            .Include(a => a.MangaSources)
                .ThenInclude(a => a.Source)
            .Include(a => a.Chapters.OrderByDescending(b => b.Date))
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Manga?> GetByMalIdAsync(int malId)
    {
        return await _context.Mangas
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.MyAnimeListId == malId);
    }
}
