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

    public void Add(MangaAuthor mangaAuthor)
    {
        _mangaAuthorRepository.Create(mangaAuthor);
    }

    public void BulkCreate(IEnumerable<MangaAuthor> mangaAuthors)
    {
        _mangaAuthorRepository.BulkCreate(mangaAuthors);
    }

    public async Task SaveChanges()
    {
        await _mangaAuthorRepository.SaveAsync();
    }
}