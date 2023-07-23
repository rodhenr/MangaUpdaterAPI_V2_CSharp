using CleanArchMvc.Domain.Interfaces;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Services;
public class MangaService : IMangaService
{
    public readonly IMangaRepository _mangaRepository;

    public MangaService(IMangaRepository mangaRepository)
    {
        _mangaRepository = mangaRepository;
    }

    public async Task AddManga(Manga manga)
    {
        await _mangaRepository.CreateAsync(manga);
    }

    public async Task<Manga> GetMangaById(int id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);

        return manga;
    }

    public async Task<IEnumerable<Manga>> GetMangas()
    {
        var mangas = await _mangaRepository.GetMangasAsync();

        return mangas;
    }

    public async Task<IEnumerable<Manga>> GetUserMangas(IEnumerable<int> ids)
    {
        var mangas = await _mangaRepository.GetListByIdAsync(ids);

        return mangas;
    }
}
