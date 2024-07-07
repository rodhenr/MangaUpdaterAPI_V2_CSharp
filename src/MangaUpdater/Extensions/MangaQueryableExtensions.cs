using MangaUpdater.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Extensions;

public static class MangaQueryableExtensions
{
    public static async Task<Manga?> GetById(this DbSet<Manga> manga, int mangaId, CancellationToken cancellationToken)
    {
        return await manga
            .AsNoTracking()
            .Where(x => x.MyAnimeListId == mangaId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}