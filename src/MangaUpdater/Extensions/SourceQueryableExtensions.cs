using MangaUpdater.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Extensions;

public static class SourceQueryableExtensions
{
    public static async Task<Source?> GetById(this DbSet<Source> source, int sourceId, CancellationToken cancellationToken)
    {
        return await source
            .AsNoTracking()
            .Where(x => x.Id == sourceId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}