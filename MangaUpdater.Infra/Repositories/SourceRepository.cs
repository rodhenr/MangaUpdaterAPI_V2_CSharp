using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class SourceRepository : BaseRepository<Source>, ISourceRepository
{
    public SourceRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public override async Task<Source?> GetByIdAsync(int id)
    {
        return await Get()
            .SingleOrDefaultAsync(s => s.Id == id);
    }
}