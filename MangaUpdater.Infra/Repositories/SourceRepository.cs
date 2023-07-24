using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Infra.Data.Repositories;

public class SourceRepository : ISourceRepository
{
    public Task<IEnumerable<Source>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Source> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
