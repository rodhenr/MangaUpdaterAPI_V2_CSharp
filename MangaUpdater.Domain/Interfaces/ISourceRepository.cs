using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface ISourceRepository
{
    Task<IEnumerable<Source>> GetSourcesAsync();
    Task<Source?> GetByIdAsync(int id);
}
