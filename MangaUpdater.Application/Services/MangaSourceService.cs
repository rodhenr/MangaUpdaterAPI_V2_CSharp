using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaSourceService: IMangaSourceService
{
    private readonly IMangaSourceRepository _mangaSourceRepository;

    public MangaSourceService(IMangaSourceRepository mangaSourceRepository)
    {
        _mangaSourceRepository = mangaSourceRepository;
    }

    public async Task<IEnumerable<MangaSource>> GetAllByMangaId(int mangaId)
    {
        return await _mangaSourceRepository.GetAllByMangaIdAsync(mangaId);
    }
}
