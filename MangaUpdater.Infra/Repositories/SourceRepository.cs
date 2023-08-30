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

    public Task CreateAsync(Source entity)
    {
        throw new NotImplementedException();
    }

    public Task BulkCreateAsync(IEnumerable<Source> entities)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Source>> GetAllAsync()
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

    public Task DeleteAsync(Source entity)
    {
        throw new NotImplementedException();
    }
}