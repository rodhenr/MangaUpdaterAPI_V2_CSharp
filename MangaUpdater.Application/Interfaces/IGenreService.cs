using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetGenresByListId(IEnumerable<int> genreIdList);
    Task<IEnumerable<Genre>> Get();
}