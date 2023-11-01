using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaGenreRepository : BaseRepository<MangaGenre>, IMangaGenreRepository
{
    public MangaGenreRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public async Task<IEnumerable<int>> GetUniqueGenreIdListAsync()
    {
        return await Get()
            .Select(mg => mg.GenreId)
            .Distinct()
            .ToListAsync();
    }

    public void BulkCreate(IEnumerable<MangaGenre> mangaGenres)
    {
        Context.MangaGenres.AddRange(mangaGenres);
    }
}