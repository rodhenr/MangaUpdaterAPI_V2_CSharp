using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
{
    public ChapterRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public async Task BulkCreateAsync(IEnumerable<Chapter> chapters)
    {
        await Context.Chapters.AddRangeAsync(chapters);
        await Context.SaveChangesAsync();
    }

    public override IQueryable<Chapter> Get()
    {
        return Context.Set<Chapter>()
            .Include(ch => ch.Source);
    }

    public async Task<IEnumerable<Chapter>> GetByMangaIdAsync(int mangaId, int max)
    {
        if (max == 0)
            return await Get()
                .Where(ch => ch.Id == mangaId)
                .ToListAsync();

        return await Get()
            .Where(ch => ch.Id == mangaId)
            .TakeLast(max)
            .ToListAsync();
    }

    public async Task<ICollection<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && sourceList.Contains(ch.SourceId))
            .OrderByDescending(ch => ch.Date)
            .Take(3)
            .ToListAsync();
    }

    public async Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .OrderBy(ch => ch.Number)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<float>> GetChaptersNumberByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .Select(ch => ch.Number)
            .ToListAsync();
    }
}