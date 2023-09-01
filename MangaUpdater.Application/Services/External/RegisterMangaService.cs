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

    public RegisterMangaService(IMyAnimeListApiService malApiService, IMapper mapper, IMangaRepository mangaRepository)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangaRepository = mangaRepository;
    }

    public async Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId)
    {
        var data = await _malApiService.GetMangaByIdAsync(malMangaId);

        var mangaInfo = _mapper.Map<Manga>(data);

        //Add Genres

        if (mangaInfo == null)
            return null;

        _mangaRepository.CreateAsync(mangaInfo);
        await _mangaRepository.SaveAsync();

        return mangaInfo;
    }
}