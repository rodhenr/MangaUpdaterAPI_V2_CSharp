using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface ISourceService
{
    Task<IEnumerable<Source>> GetSources();
    Task<Source?> GetById(int id);
}
