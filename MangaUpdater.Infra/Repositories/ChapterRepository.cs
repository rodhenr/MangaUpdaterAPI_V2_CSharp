using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Application.Helpers;

namespace MangaUpdater.Infra.Data.Repositories;

public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
{
    public ChapterRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public void BulkCreate(IEnumerable<Chapter> chapters)
    {
        Context.Chapters.AddRange(chapters);
    }

    public override async Task<Chapter?> GetByIdAsync(int id)
    {
        return await Get()
            .Include(ch => ch.Source)
            .AsNoTracking()
            .SingleOrDefaultAsync(ch => ch.Id == id);
    }

    public async Task<Chapter?> GetLastChapterByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        var chapters = await Get()
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .AsNoTracking()
            .ToListAsync();

        chapters
            .Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
                .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));
            
        return chapters.LastOrDefault();
    }

    public async Task<IEnumerable<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && sourceList.Contains(ch.SourceId))
            .Include(ch => ch.Source)
            .OrderByDescending(ch => ch.Date)
            .Take(3)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .OrderBy(ch => ch.Number)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}