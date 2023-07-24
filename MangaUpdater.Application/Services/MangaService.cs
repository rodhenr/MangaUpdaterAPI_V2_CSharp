using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;
public class MangaService : IMangaService
{
    private readonly IMangaRepository _mangaRepository;

    public MangaService(IMangaRepository mangaRepository)
    {
        _mangaRepository = mangaRepository;
    }

    public async Task AddManga(Manga manga)
    {
        await _mangaRepository.CreateMangaAsync(manga);
    }

    public async Task<Manga> GetMangaById(int id)
    {
        return await _mangaRepository.GetMangaByIdAsync(id);
    }

    public async Task<IEnumerable<Manga>> GetMangas()
    {
        return await _mangaRepository.GetMangasAsync();
    }

    public Task<IEnumerable<Manga>> GetUserMangas(int userId)
    {
        throw new NotImplementedException();
    }
}
