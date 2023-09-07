using MangaUpdater.Application.Helpers;
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

    public void Add(MangaSource mangaSource)
    {
        _mangaSourceRepository.CreateAsync(mangaSource);
    }

    public async Task<ICollection<MangaSource>> GetAllByMangaId(int mangaId)
    {
        return await _mangaSourceRepository.GetAllByMangaIdAsync(mangaId);
    }

    public async Task<MangaSource> GetByMangaIdAndSourceId(int mangaId, int sourceId)
    {
        var mangaSource = await _mangaSourceRepository.GetByMangaIdAndSourceIdAsync(mangaId, sourceId);
        ValidationHelper.ValidateEntity(mangaSource);

        return mangaSource;
    }

    public async Task SaveChanges()
    {
        await _mangaSourceRepository.SaveAsync();
    }
}