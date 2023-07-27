using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IChapterRepository
{
    Task CreateAsync(Chapter chapter);
    Task<IEnumerable<Chapter>> GetChaptersByIdAsync(int mangaId, int max);
}
