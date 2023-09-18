using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaAuthorRepository : BaseRepository<MangaAuthor>, IMangaAuthorRepository
{
    public MangaAuthorRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors)
    {
        Context.MangaAuthors.AddRange(mangaAuthors);
    }
}