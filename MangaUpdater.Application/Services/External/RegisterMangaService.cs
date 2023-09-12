using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services.External;

public class RegisterMangaService : IRegisterMangaService
{
    private readonly IMyAnimeListApiService _malApiService;
    private readonly IMapper _mapper;
    private readonly IMangaRepository _mangaRepository;
    private readonly IMangaGenreRepository _mangaGenreRepository;

    public RegisterMangaService(IMyAnimeListApiService malApiService, IMapper mapper, IMangaRepository mangaRepository,
        IMangaGenreRepository mangaGenreRepository)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangaRepository = mangaRepository;
        _mangaGenreRepository = mangaGenreRepository;
    }

    public async Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId)
    {
        var manga = await _mangaRepository.GetByMalIdAsync(malMangaId);

        if (manga is not null) throw new Exception("Manga already registered");

        var apiData = await _malApiService.GetMangaByIdAsync(malMangaId);

        var mangaInfo = _mapper.Map<Manga>(apiData);

        _mangaRepository.CreateAsync(mangaInfo);
        await _mangaRepository.SaveAsync();

        var mangaGenreList =
            apiData!.Genres.Select(g => new MangaGenre() { GenreId = (int)g.MalId, MangaId = mangaInfo.Id });

        _mangaGenreRepository.BulkCreateAsync(mangaGenreList);

        return mangaInfo;
    }
}