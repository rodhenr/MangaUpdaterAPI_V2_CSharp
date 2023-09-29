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
    
    public void BulkCreate(IEnumerable<MangaTitle> mangaTitles)
    {
        _mangaTitleRepository.BulkCreate(mangaTitles);
    }
}