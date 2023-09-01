using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaSourceRepository : BaseRepository<MangaSource>,IMangaSourceRepository
{
    public MangaSourceRepository(IdentityMangaUpdaterContext context): base(context)
    {
    }

    public async Task<ICollection<MangaSource>> GetAllByMangaIdAsync(int mangaId)
    {
        return await Get()
            .Where(ms => ms.MangaId == mangaId)
            .Include(ms => ms.Source)
            .Include(ms => ms.Manga)
            .ToListAsync();
    }

    public async Task<MangaSource?> GetByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        return await Get()
            .Where(ms => ms.MangaId == mangaId && ms.SourceId == sourceId)
            .Include(ms => ms.Source)
            .SingleOrDefaultAsync();
    }
}