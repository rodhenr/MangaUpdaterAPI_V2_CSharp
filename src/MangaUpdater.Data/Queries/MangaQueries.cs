using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Queries;

public static class MangaQueries
{
    public static async Task<Manga?> GetById(this DbSet<Manga> manga, int mangaId, CancellationToken cancellationToken)
    {
        return await manga
            .AsNoTracking()
            .Where(x => x.Id == mangaId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}