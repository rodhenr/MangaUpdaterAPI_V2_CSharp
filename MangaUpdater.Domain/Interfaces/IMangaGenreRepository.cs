using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaGenreRepository: IBaseRepository<MangaGenre>
{
    Task<IEnumerable<int>> GetUniqueGenreIdListAsync();
    void BulkCreate(IEnumerable<MangaGenre> mangaGenres);
}