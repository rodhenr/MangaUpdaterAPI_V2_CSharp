using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface ISourceService
{
    Task<IEnumerable<Source>> Get();
    Task<Source> GetById(int id);
}