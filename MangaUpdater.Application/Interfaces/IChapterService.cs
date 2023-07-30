using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IChapterService
{
    Task AddChapter(Chapter chapter);

    Task<IEnumerable<Chapter>> GetChaptersByMangaId(int mangaId, int? max);
    Task<Chapter?> GetChapterById(int id);
}
