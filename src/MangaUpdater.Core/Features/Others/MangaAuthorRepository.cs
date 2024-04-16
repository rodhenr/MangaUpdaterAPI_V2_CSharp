using MangaUpdater.Data.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Data;

namespace MangaUpdater.Data.Repositories;

public class MangaAuthorRepository : BaseRepository<MangaAuthor>, IMangaAuthorRepository
{
    public MangaAuthorRepository(AppDbContextIdentity context) : base(context)
    {
    }

    public void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors)
    {
        Context.MangaAuthors.AddRange(mangaAuthors);
    }
}