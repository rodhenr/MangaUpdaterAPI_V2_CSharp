using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaAuthorService
{
    void Add(MangaAuthor mangaAuthor);
    void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors);
    Task SaveChanges();
}