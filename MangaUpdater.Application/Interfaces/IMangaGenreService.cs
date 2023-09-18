using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaGenreService
{
    void Add(MangaGenre mangaGenre);
    void BulkCreate(IEnumerable<MangaGenre> mangaGenres);
    Task SaveChanges();
}