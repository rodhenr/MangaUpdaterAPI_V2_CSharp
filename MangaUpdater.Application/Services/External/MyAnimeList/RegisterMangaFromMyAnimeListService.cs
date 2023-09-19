using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Application.Models.External.MyAnimeList;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.Application.Services.External.MyAnimeList;

public class RegisterMangaFromMyAnimeListService : IRegisterMangaFromMyAnimeListService
{
    private readonly IMyAnimeListApiService _malApiService;
    private readonly IMapper _mapper;
    private readonly IMangaService _mangaService;
    private readonly IMangaGenreService _mangaGenreService;
    private readonly IMangaAuthorService _mangaAuthorService;
    private readonly IMangaTitleService _mangaTitleService;

    public RegisterMangaFromMyAnimeListService(IMyAnimeListApiService malApiService, IMapper mapper, IMangaService mangaService,
        IMangaGenreService mangaGenreService, IMangaAuthorService mangaAuthorService,
        IMangaTitleService mangaTitleService)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangaService = mangaService;
        _mangaGenreService = mangaGenreService;
        _mangaAuthorService = mangaAuthorService;
        _mangaTitleService = mangaTitleService;
    }

    public async Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId)
    {
        var isMangaRegistered = await _mangaService.CheckIfMangaIsRegistered(malMangaId);
        if (isMangaRegistered) throw new BadRequestException($"The ID {_malApiService} is already registered");

        var apiData = await _malApiService.GetMangaFromMyAnimeListByIdAsync(malMangaId);
        var manga = _mapper.Map<Manga>(apiData);
        await _mangaService.Add(manga); //TODO: Create a single transaction

        await CreateAditionalMangaData(apiData!, manga);

        return manga;
    }

    private async Task CreateAditionalMangaData(MyAnimeListApiResponse apiData, Manga manga)
    {
        var genreList =
            apiData!.Genres
                .Select(g => new MangaGenre() { GenreId = (int)g.MalId, MangaId = manga.Id });

        var titleList = apiData.Titles
            .Select(t => new MangaTitle() { MangaId = manga.Id, Name = t.Title });

        var authorList = apiData.Authors
            .Select(a => new MangaAuthor() { MangaId = manga.Id, Name = a.Name });

        _mangaGenreService.BulkCreate(genreList);
        _mangaAuthorService.BulkCreate(authorList);
        _mangaTitleService.BulkCreate(titleList);

        await _mangaGenreService.SaveChanges();
    }
}