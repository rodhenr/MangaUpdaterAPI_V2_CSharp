using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Infra.Data.Repositories;

public class GenreRepository : IGenreRepository
{
    public Task<IEnumerable<Genre>> GetGenres()
    {
        throw new NotImplementedException();
    }
}
