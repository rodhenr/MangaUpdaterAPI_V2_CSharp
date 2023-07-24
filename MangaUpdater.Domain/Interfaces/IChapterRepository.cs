using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IChapterRepository
{
    Task CreateAsync(int mangaId, int sourceId, DateTime date, float chapterNumber);
    Task<IEnumerable<Chapter>> GetByIdAsync(int mangaId, int? max);
}
