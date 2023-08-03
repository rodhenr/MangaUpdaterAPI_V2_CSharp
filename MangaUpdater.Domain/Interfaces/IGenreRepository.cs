using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAsync();
}
