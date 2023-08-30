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

    public async Task CreateAsync(Source entity)
    {
        await _context.Sources.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Source>> GetAsync()
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

    public async Task DeleteAsync(Source entity)
    {
        _context.Sources.Remove(entity);
        await _context.SaveChangesAsync();
    }
}