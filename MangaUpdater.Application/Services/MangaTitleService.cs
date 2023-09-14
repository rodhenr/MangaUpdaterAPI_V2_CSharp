using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaTitleService : IMangaTitleService
{
    private readonly IMangaTitleRepository _mangaTitleRepository;

    public MangaTitleService(IMangaTitleRepository mangaTitleRepository)
    {
        _mangaTitleRepository = mangaTitleRepository;
    }

    public async Task<IEnumerable<MangaTitle>> GetAllByMangaId(int mangaId)
    {
        return await _mangaTitleRepository.GetByMangaIdAsync(mangaId);
    }

    public async Task SaveChanges()
    {
        await _mangaTitleRepository.SaveAsync();
    }
}