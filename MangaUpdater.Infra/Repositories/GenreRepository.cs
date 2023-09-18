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
    
    public void BulkCreate(IEnumerable<Genre> genres)
    {
        Context.Genres.AddRange(genres);
    }

    public override async Task<Genre?> GetByIdAsync(int id)
    {
        return await Get()
            .AsNoTracking()
            .SingleOrDefaultAsync(g => g.Id == id);
    }
}