﻿using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly MangaUpdaterContext _context;

    public SourceRepository(MangaUpdaterContext context)
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
            .SingleOrDefaultAsync(a => a.Id == id);
    }
}
