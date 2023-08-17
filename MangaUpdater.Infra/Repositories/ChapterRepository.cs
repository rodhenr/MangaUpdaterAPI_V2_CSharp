using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class ChapterRepository : IChapterRepository
{
    private readonly MangaUpdaterContext _context;

    public ChapterRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Chapter chapter)
    {
        await _context.AddAsync(chapter);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task<Chapter?> GetByIdAsync(int id)
    {
        return await _context.Chapters
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Chapter>> GetAllByMangaIdAsync(int mangaId, int max)
    {
        if (max == 0)
        {
            return await _context.Chapters
                .Where(a => a.Id == mangaId)
                .AsNoTracking()
                .ToListAsync();
        }

        return await _context.Chapters
            .Where(a => a.Id == mangaId)
            .TakeLast(max)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<Chapter>> GetThreeLastByMangaIdAndSourceListAsync(int mangaId, List<int> sourceList)
    {
        return await _context.Chapters
            .Where(a => a.MangaId == mangaId && sourceList.Contains(a.SourceId))
            .Include(ch => ch.Source)
            .OrderByDescending(a => a.Date)
            .Take(3)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Chapter?> GetSmallestChapterByMangaIdAsync(int mangaId, int sourceId)
    {
        return await _context.Chapters
            .Where(a => a.MangaId == mangaId && a.SourceId == sourceId)
            .OrderBy(a => a.Number)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
