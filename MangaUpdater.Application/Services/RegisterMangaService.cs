using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;
public class RegisterMangaService: IRegisterMangaService
{
    private readonly IMyAnimeListAPIService _malApiService;
    private readonly IMapper _mapper;
    private readonly IMangaRepository _mangaRepository;

    public RegisterMangaService(IMyAnimeListAPIService malApiService, IMapper mapper, IMangaRepository mangaRepository)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangaRepository = mangaRepository;
    }

    public async Task<Manga?> RegisterMangaFromMyAnimeListById(int malMangaId)
    {
        MyAnimeListAPIResponse? data = await _malApiService.GetMangaByIdAsync(malMangaId);

        Manga? mangaInfo = _mapper.Map<Manga>(data);

        //Add Genres

        if (mangaInfo == null)
        {
            return null;
        }

        await _mangaRepository.CreateAsync(mangaInfo);

        return mangaInfo;
    }
}
