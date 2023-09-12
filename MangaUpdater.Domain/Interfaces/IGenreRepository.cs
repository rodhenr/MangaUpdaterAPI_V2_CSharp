using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IGenreRepository: IBaseRepository<Genre>
{
    void BulkCreateAsync(IEnumerable<Genre> genres);
}