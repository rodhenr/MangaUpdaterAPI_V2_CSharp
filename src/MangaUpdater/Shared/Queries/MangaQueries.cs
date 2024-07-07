using MangaUpdater.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infrastructure.Queries;

public static class MangaQueries
{
    public static async Task<Manga?> GetById(this DbSet<Manga> manga, int mangaId, CancellationToken cancellationToken)
    {
        return await manga
            .AsNoTracking()
            .Where(x => x.MyAnimeListId == mangaId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}