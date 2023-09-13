using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaAuthorRepository: IBaseRepository<MangaAuthor>
{
    void BulkCreateAsync(IEnumerable<MangaAuthor> mangaAuthors);
}