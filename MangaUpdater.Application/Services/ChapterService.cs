using MangaUpdater.Application.Helpers;
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

    public void Add(Chapter chapter)
    {
        _chapterRepository.Create(chapter);
    }

    public void BulkCreate(IEnumerable<Chapter> chapters)
    {
        _chapterRepository.BulkCreate(chapters);
    }

    public async Task<Chapter> GetById(int id)
    {
        var chapter = await _chapterRepository.GetByIdAsync(id);
        ValidationHelper.ValidateEntity(chapter);

        return chapter;
    }

    public async Task<IEnumerable<Chapter>> GetByMangaId(int mangaId, int? max)
    {
        return await _chapterRepository.GetByMangaIdAsync(mangaId, max ?? 0);
    }

    public async Task CreateOrUpdateByMangaSource(int mangaId, int sourceId,
        Dictionary<string, string> chapters)
    {
        var chaptersInDatabase = await _chapterRepository.GetChaptersNumberByMangaIdAndSourceIdAsync(mangaId, sourceId);

        List<Chapter> chaptersToUpdate = (from chapter in chapters
            where !chaptersInDatabase.Any(ch => Math.Abs(ch - float.Parse(chapter.Key)) < 0)
            select new Chapter
            {
                MangaId = mangaId,
                SourceId = sourceId,
                Date = DateTime.Parse(chapter.Value),
                Number = float.Parse(chapter.Key)
            }).ToList();

        _chapterRepository.BulkCreate(chaptersToUpdate);
        await _chapterRepository.SaveAsync();
    }

    public async Task<Chapter?> GetLastByMangaIdAndSourceId(int mangaId, int sourceId)
    {
        return await _chapterRepository.GetLastChapterByMangaIdAndSourceIdAsync(mangaId, sourceId);
    }

    public async Task SaveChanges()
    {
        await _chapterRepository.SaveAsync();
    }
}