using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class ChapterRepository : IChapterRepository
{
    private readonly IdentityMangaUpdaterContext _context;

    public ChapterRepository(IdentityMangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Chapter chapter)
    {
        await _context.AddAsync(chapter);
        await _context.SaveChangesAsync();
    }

    public async Task BulkCreateAsync(IEnumerable<Chapter> chapters)
    {
        await _context.Chapters.AddRangeAsync(chapters);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Chapter>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Chapter?> GetByIdAsync(int id)
    {
        return await _context.Chapters
            .Include(ch => ch.Source)
            .AsNoTracking()
            .SingleOrDefaultAsync(ch => ch.Id == id);
    }

    public Task DeleteAsync(Chapter entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Chapter>> GetByMangaIdAsync(int mangaId, int max)
    {
        if (max == 0)
            return await _context.Chapters
                .Where(ch => ch.Id == mangaId)
                .AsNoTracking()
                .ToListAsync();

        return await _context.Chapters
            .Where(ch => ch.Id == mangaId)
            .TakeLast(max)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList)
    {
        return await _context.Chapters
            .Where(ch => ch.MangaId == mangaId && sourceList.Contains(ch.SourceId))
            .Include(ch => ch.Source)
            .OrderByDescending(ch => ch.Date)
            .Take(3)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId)
    {
        return await _context.Chapters
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .OrderBy(ch => ch.Number)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<float>> GetChaptersNumberByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        return await _context.Chapters
            .Where(ch => ch.MangaId == mangaId && ch.SourceId == sourceId)
            .Select(ch => ch.Number)
            .ToListAsync();
    }
}