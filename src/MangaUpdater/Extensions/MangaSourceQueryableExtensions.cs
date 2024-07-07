using MangaUpdater.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Extensions;

public static class MangaSourceQueryableExtensions
{
    public static async Task<MangaSource?> GetMangaSourceQueryable(this IQueryable<MangaSource> mangaSources, int mangaId, int sourceId, CancellationToken cancellationToken = default)
    {
        return await mangaSources
            .Where(x => x.MangaId == mangaId && x.SourceId == sourceId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}