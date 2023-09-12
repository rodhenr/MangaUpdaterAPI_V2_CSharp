using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaGenreRepository: IBaseRepository<MangaGenre>
{
    void BulkCreateAsync(IEnumerable<MangaGenre> mangaGenres);
}