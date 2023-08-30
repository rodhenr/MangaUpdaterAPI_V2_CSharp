using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaSourceRepository : IMangaSourceRepository
{
    private readonly IdentityMangaUpdaterContext _context;

    public MangaSourceRepository(IdentityMangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(MangaSource mangaSource)
    {
        await _context.AddAsync(mangaSource);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<MangaSource>> GetAllByMangaIdAsync(int mangaId)
    {
        return await _context.MangaSources
            .Where(ms => ms.MangaId == mangaId)
            .Include(ms => ms.Source)
            .Include(ms => ms.Manga)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<MangaSource>> GetAllBySourceIdAsync(int sourceId)
    {
        return await _context.MangaSources
            .Where(ms => ms.SourceId == sourceId)
            .Include(ms => ms.Source)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<MangaSource?> GetByMangaIdAndSourceIdAsync(int mangaId, int sourceId)
    {
        return await _context.MangaSources
            .Where(ms => ms.MangaId == mangaId && ms.SourceId == sourceId)
            .Include(ms => ms.Source)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
}