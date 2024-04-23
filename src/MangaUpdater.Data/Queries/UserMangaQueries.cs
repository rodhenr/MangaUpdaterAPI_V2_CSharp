using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Queries;

public static class UserMangaQueries
{
    public static async Task<UserManga?> GetByMangaIdAndUserId(this DbSet<UserManga> userManga, int mangaId, string userId, CancellationToken cancellationToken)
    {
        return await userManga
            .AsNoTracking()
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}