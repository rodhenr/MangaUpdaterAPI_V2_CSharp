using MangaUpdater.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infrastructure.Queries;

public static class SourceQueries
{
    public static async Task<Source?> GetById(this DbSet<Source> source, int sourceId, CancellationToken cancellationToken)
    {
        return await source
            .AsNoTracking()
            .Where(x => x.Id == sourceId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}