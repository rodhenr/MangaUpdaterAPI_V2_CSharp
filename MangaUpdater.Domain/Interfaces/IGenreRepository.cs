using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IGenreRepository: IBaseRepository<Genre>
{
    Task<IEnumerable<Genre>> GetGenresByListIdAsync(IEnumerable<int> genreIdList);
}