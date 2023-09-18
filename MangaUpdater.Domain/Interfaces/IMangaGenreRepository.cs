using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaGenreRepository: IBaseRepository<MangaGenre>
{
    void BulkCreate(IEnumerable<MangaGenre> mangaGenres);
}