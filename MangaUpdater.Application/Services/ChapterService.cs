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

    public async Task AddChapter(Chapter chapter)
    {
        await _chapterRepository.CreateAsync(chapter);
    }

    public async Task BulkCreate(List<Chapter> chapters)
    {
        await _chapterRepository.BulkCreateAsync(chapters);
    }

    public async Task<Chapter?> GetChapterById(int id)
    {
        return await _chapterRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Chapter>> GetChaptersByMangaId(int mangaId, int? max)
    {
        return await _chapterRepository.GetByMangaIdAsync(mangaId, max ?? 0);
    }

    public async Task CreateOrUpdateChaptersByMangaSource(int mangaId, int sourceId,
        Dictionary<string, string> chapters)
    {
        var chaptersInDatabase = await _chapterRepository.GetChaptersNumberByMangaIdAndSourceIdAsync(mangaId, sourceId);

        List<Chapter> chaptersToUpdate = (from chapter in chapters
            where !chaptersInDatabase.Any(ch => Math.Abs(ch - float.Parse(chapter.Key)) < 0)
            select new Chapter(mangaId, sourceId, DateTime.Parse(chapter.Value), float.Parse(chapter.Key))).ToList();

        await _chapterRepository.BulkCreateAsync(chaptersToUpdate);
    }
}

// foreach (var chapter in chapters)
// {
//     if (!chaptersInDatabase.Any(c => c == float.Parse(chapter.Key)))
//     {
//         chaptersToUpdate.Add(new Chapter(mangaId, sourceId, DateTime.Parse(chapter.Value),
//             float.Parse(chapter.Key)));
//     }
// }