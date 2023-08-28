using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaSourceService : IMangaSourceService
{
    private readonly IMangaSourceRepository _mangaSourceRepository;

    public MangaSourceService(IMangaSourceRepository mangaSourceRepository)
    {
        _mangaSourceRepository = mangaSourceRepository;
    }

    public async Task AddMangaSource(MangaSource mangaSource)
    {
        await _mangaSourceRepository.CreateAsync(mangaSource);
    }

    public async Task<ICollection<MangaSource>> GetAllByMangaId(int mangaId)
    {
        return await _mangaSourceRepository.GetAllByMangaIdAsync(mangaId);
    }

    public async Task<MangaSource?> GetByMangaIdAndSourceId(int mangaId, int sourceId)
    {
        return await _mangaSourceRepository.GetByMangaIdAndSourceIdAsync(mangaId, sourceId);
    }
}