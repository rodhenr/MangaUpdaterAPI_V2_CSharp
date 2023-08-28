using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly IdentityMangaUpdaterContext _context;

    public SourceRepository(IdentityMangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Source>> GetAsync()
    {
        return await _context.Sources
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Source?> GetByIdAsync(int id)
    {
        return await _context.Sources
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.Id == id);
    }
}