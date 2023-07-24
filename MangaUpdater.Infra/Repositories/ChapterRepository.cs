using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Infra.Data.Repositories;

public class ChapterRepository : IChapterRepository
{
    public Task CreateAsync(int mangaId, int sourceId, DateTime date, float chapterNumber)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Chapter>> GetByIdAsync(int mangaId, int? max)
    {
        throw new NotImplementedException();
    }
}
