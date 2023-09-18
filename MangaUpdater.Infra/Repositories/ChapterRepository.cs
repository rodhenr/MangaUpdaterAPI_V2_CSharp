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

    public async Task<IEnumerable<Chapter>> GetByMangaIdAsync(int mangaId, int max = 0)
    {
        if (max == 0)
            return await Get()
                .Where(ch => ch.Id == mangaId)
                .AsNoTracking()
                .ToListAsync();

        return await Get()
            .Where(ch => ch.Id == mangaId)
            .TakeLast(max)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Chapter?> GetLastChapterByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .OrderBy(ch => ch.Number)
            .OrderDescending()
            .AsNoTracking()
            .FirstOrDefaultAsync();
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

    public async Task<IEnumerable<float>> GetChaptersNumberByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        return await Get()
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .AsNoTracking()
            .Select(ch => ch.Number)
            .ToListAsync();
    }
}