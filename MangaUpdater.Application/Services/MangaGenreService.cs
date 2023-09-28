using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaGenreService : IMangaGenreService
{
    private readonly IMangaGenreRepository _mangaGenreRepository;

    public MangaGenreService(IMangaGenreRepository mangaGenreRepository)
    {
        _mangaGenreRepository = mangaGenreRepository;
    }
    
    public void BulkCreate(IEnumerable<MangaGenre> mangaGenres)
    {
        _mangaGenreRepository.BulkCreate(mangaGenres);
    }

    public async Task SaveChanges()
    {
        await _mangaGenreRepository.SaveAsync();
    }
}