using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaRepository : BaseRepository<Manga>, IMangaRepository
{
    public MangaRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public override async Task<Manga?> GetByIdAsync(int id)
    {
        return await Get()
            .Include(m => m.MangaSources)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Manga?> GetByMalIdAsync(int malId)
    {
        return await Get()
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.MyAnimeListId == malId);
    }

    public async Task<IEnumerable<Manga>> GetWithFiltersAsync(string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList)
    {
        var query = Get();

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
        return await Get()
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
        return await Get()
            .Include(m => m.UserMangas.Where(um => um.UserId == userId))
            .Include(m => m.MangaGenres)
            .ThenInclude(m => m.Genre)
            .Include(m => m.MangaSources)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters.OrderByDescending(ch => ch.Date))
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
    }
}