using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services.External;

public class RegisterMangaService : IRegisterMangaService
{
    private readonly IMyAnimeListApiService _malApiService;
    private readonly IMapper _mapper;
    private readonly IMangaRepository _mangaRepository;
    private readonly IMangaGenreRepository _mangaGenreRepository;
    private readonly IMangaAuthorRepository _mangaAuthorRepository;
    private readonly IMangaTitleRepository _mangaTitleRepository;

    public RegisterMangaService(IMyAnimeListApiService malApiService, IMapper mapper, IMangaRepository mangaRepository,
        IMangaGenreRepository mangaGenreRepository, IMangaAuthorRepository mangaAuthorRepository,
        IMangaTitleRepository mangaTitleRepository)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangaRepository = mangaRepository;
        _mangaGenreRepository = mangaGenreRepository;
        _mangaAuthorRepository = mangaAuthorRepository;
        _mangaTitleRepository = mangaTitleRepository;
    }

    public async Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId)
    {
        var manga = await _mangaRepository.GetByMalIdAsync(malMangaId);

        if (manga is not null) throw new Exception("Manga already registered");

        var apiData = await _malApiService.GetMangaByIdAsync(malMangaId);

        var mangaInfo = _mapper.Map<Manga>(apiData);

        _mangaRepository.CreateAsync(mangaInfo);
        await _mangaRepository.SaveAsync();

        var genreList =
            apiData!.Genres.Select(g => new MangaGenre() { GenreId = (int)g.MalId, MangaId = mangaInfo.Id });

        var titleList = apiData!.Titles.Select(t => new MangaTitle() { MangaId = mangaInfo.Id, Name = t.Title });

        var authorList = apiData!.Authors.Select(a => new MangaAuthor() { MangaId = mangaInfo.Id, Name = a.Name });

        _mangaGenreRepository.BulkCreateAsync(genreList);
        _mangaAuthorRepository.BulkCreateAsync(authorList);
        _mangaTitleRepository.BulkCreateAsync(titleList);

        await _mangaRepository.SaveAsync();
        return mangaInfo;
    }
}