using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly IdentityMangaUpdaterContext _context;

    public MangaRepository(IdentityMangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Manga manga)
    {
        await _context.AddAsync(manga);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Manga>> GetAsync()
    {
        return await _context.Mangas
            .Include(m => m.MangaSources)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Manga?> GetByIdAsync(int id)
    {
        return await _context.Mangas
            .Include(m => m.MangaSources)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task DeleteAsync(Manga entity)
    {
        _context.Mangas.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Manga>> GetWithFiltersAsync(string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList)
    {
        var query = _context.Mangas
            .AsQueryable();

        query = orderBy switch
        {
            "alphabet" => query.OrderBy(m => m.Name),
            "latest" => query.OrderByDescending(m => m.Id),
            _ => query
        };

        if (sourceIdList != null && sourceIdList.Any())
            query = query.Where(m =>
                m.MangaSources != null && m.MangaSources.Any(b => sourceIdList.Contains(b.SourceId)));

        if (genreIdList != null && genreIdList.Any())
            query = query.Where(m => m.MangaGenres != null && m.MangaGenres.Any(b => genreIdList.Contains(b.GenreId)));

        return await query
            .Include(m => m.MangaSources)
            .Include(m => m.MangaGenres)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Manga?> GetByIdOrderedDescAsync(int id)
    {
        return await _context.Mangas
            .Include(m => m.MangaGenres)
            .ThenInclude(mg => mg.Genre)
            .Include(m => m.MangaSources)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters.OrderByDescending(ch => ch.Date))
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, string userId)
    {
        return await _context.Mangas
            .Include(m => m.UserMangas.Where(um => um.UserId == userId))
            .Include(m => m.MangaGenres)
            .ThenInclude(m => m.Genre)
            .Include(m => m.MangaSources)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters.OrderByDescending(ch => ch.Date))
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Manga?> GetByMalIdAsync(int malId)
    {
        return await _context.Mangas
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.MyAnimeListId == malId);
    }
}