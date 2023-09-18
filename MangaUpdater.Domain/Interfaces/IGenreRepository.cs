using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IGenreRepository: IBaseRepository<Genre>
{
    void BulkCreate(IEnumerable<Genre> genres);
}