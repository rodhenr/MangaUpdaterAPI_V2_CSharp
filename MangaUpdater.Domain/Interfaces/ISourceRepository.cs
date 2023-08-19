using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface ISourceRepository
{
    Task<ICollection<Source>> GetAsync();
    Task<Source?> GetByIdAsync(int id);
}
