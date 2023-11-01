using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaGenreService
{
    Task<IEnumerable<int>> GetUniqueGenresId();
    void BulkCreate(IEnumerable<MangaGenre> mangaGenres);
    Task SaveChanges();
}