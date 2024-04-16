using MangaUpdater.Data.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaAuthorService
{
    void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors);
}