using MangaUpdater.Data.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Data;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Repositories;

public class MangaGenreRepository : BaseRepository<MangaGenre>, IMangaGenreRepository
{
    public MangaGenreRepository(AppDbContextIdentity context) : base(context)
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