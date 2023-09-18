using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaGenreRepository : BaseRepository<MangaGenre>, IMangaGenreRepository
{
    public MangaGenreRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public void BulkCreate(IEnumerable<MangaGenre> mangaGenres)
    {
        Context.MangaGenres.AddRange(mangaGenres);
    }
}