using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class GenreRepository : BaseRepository<Genre>, IGenreRepository
{
    public GenreRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public override async Task<Genre?> GetByIdAsync(int id)
    {
        return await Get()
            .AsNoTracking()
            .SingleOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Genre>> GetGenresByListIdAsync(IEnumerable<int> genreIdList)
    {
        return await Get()
            .Where(g => genreIdList.Contains(g.Id))
            .OrderBy(g => g.Name)
            .ToListAsync();
    }
}