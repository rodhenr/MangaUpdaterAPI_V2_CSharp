using MangaUpdater.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Extensions;

public static class UserMangaQueryableExtensions
{
    public static async Task<UserManga?> GetByMangaIdAndUserId(this IQueryable<UserManga> userManga, int mangaId, string userId, CancellationToken cancellationToken)
    {
        return await userManga
            .AsNoTracking()
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}