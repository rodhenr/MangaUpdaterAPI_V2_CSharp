using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaAuthorService : IMangaAuthorService
{
    private readonly IMangaAuthorRepository _mangaAuthorRepository;

    public MangaAuthorService(IMangaAuthorRepository mangaAuthorRepository)
    {
        _mangaAuthorRepository = mangaAuthorRepository;
    }
    
    public void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors)
    {
        _mangaAuthorRepository.BulkCreate(mangaAuthors);
    }
}