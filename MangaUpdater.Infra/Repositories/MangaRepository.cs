using System.Globalization;
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
            .Include(m => m.MangaAuthors)
            .Include(m => m.MangaGenres)
            .Include(m => m.MangaTitles)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Manga?> GetByMalIdAsync(int malId)
    {
        return await Get()
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.MyAnimeListId == malId);
    }

    public async Task<IEnumerable<Manga>> GetWithFiltersAsync(int page, int pageSize, string? orderBy,
        List<int>? sourceIdList,
        List<int>? genreIdList)
    {
        var query = Get()
            .Include(m => m.MangaTitles)
            .Include(m => m.MangaSources)
            .Include(m => m.MangaGenres)
            .AsQueryable();

        query = orderBy switch
        {
            "alphabet" => query
                .OrderBy(m => m.MangaTitles!.First().Name),
            "latest" => query
                .OrderByDescending(m => m.Id),
            _ => query
        };

        if (sourceIdList != null && sourceIdList.Any())
            query = query
                .Where(m => m.MangaSources != null && m.MangaSources.Any(b => sourceIdList.Contains(b.SourceId)))
                .Include(m => m.MangaSources);

        if (genreIdList != null && genreIdList.Any())
            query = query.Where(m => m.MangaGenres != null && m.MangaGenres.Any(b => genreIdList.Contains(b.GenreId)))
                .Include(m => m.MangaGenres);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Manga?> GetByIdOrderedDescAsync(int id)
    {
        var manga = await Get()
            .Include(m => m.MangaGenres!)
            .ThenInclude(mg => mg.Genre)
            .Include(m => m.MangaSources!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.MangaAuthors)
            .Include(m => m.MangaTitles)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga is not null)
            manga.Chapters =
                manga.Chapters?.OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture));

        return manga;
    }

    public async Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, string userId)
    {
        var manga = await Get()
            .Include(m => m.UserMangas!.Where(um => um.UserId == userId))
            .ThenInclude(um => um.UserChapter)
            .ThenInclude(uc => uc!.Source)
            .Include(m => m.MangaGenres)!
            .ThenInclude(m => m.Genre)
            .Include(m => m.MangaSources)!
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.MangaAuthors)
            .Include(m => m.MangaTitles)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);

        if (manga is not null)
            manga.Chapters =
                manga.Chapters?.OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture));

        return manga;
    }

    public async Task<int> CheckNumberOfPagesAsync(int pageSize)
    {
        var numberOfMangas = await Get()
            .AsNoTracking()
            .Select(m => m.Id)
            .ToListAsync();

        var numberOfPages = (int)Math.Ceiling((double)numberOfMangas.Count / pageSize);

        return numberOfPages;
    }
}