using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface ISourceService
{
    Task<ICollection<Source>> GetSources();
    Task<Source?> GetSourcesById(int id);
}