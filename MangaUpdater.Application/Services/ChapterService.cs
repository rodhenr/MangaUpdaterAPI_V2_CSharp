using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class ChapterService : IChapterService
{
    private readonly IChapterRepository _chapterRepository;

    public ChapterService(IChapterRepository chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }

    public void BulkCreate(IEnumerable<Chapter> chapters)
    {
        _chapterRepository.BulkCreate(chapters);
    }

    public async Task<Chapter?> GetLastByMangaIdAndSourceId(int mangaId, int sourceId)
    {
        return await _chapterRepository.GetLastChapterByMangaIdAndSourceIdAsync(mangaId, sourceId);
    }
}