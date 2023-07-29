using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaSourceRepository: IMangaSourceRepository
{
    private readonly MangaUpdaterContext _context;

    public MangaSourceRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MangaSource>> GetByMangaIdAsync(int mangaId)
    {
        return await _context.MangaSources
            .Where(a => a.MangaId == mangaId)
            .Include(a => a.Source)
            .ToListAsync();
    }

    public Task<IEnumerable<MangaSource>> GetBySourceIdAsync(int sourceId)
    {
        throw new NotImplementedException();
    }
}
