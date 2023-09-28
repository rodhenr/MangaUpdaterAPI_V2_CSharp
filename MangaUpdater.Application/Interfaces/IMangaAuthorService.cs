using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaAuthorService
{
    void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors);
}