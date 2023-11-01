using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<Genre>> GetGenresByListId(IEnumerable<int> genreIdList)
    {
        return await _genreRepository.GetGenresByListIdAsync(genreIdList);
    }

    public async Task<IEnumerable<Genre>> Get()
    {
        return await _genreRepository.GetAsync();
    }
}