using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaService : IMangaService
{
    private readonly IMangaRepository _mangaRepository;
    private readonly IMapper _mapper;

    public MangaService(IMangaRepository mangaRepository, IMapper mapper)
    {
        _mangaRepository = mangaRepository;
        _mapper = mapper;
    }

    public async Task AddManga(Manga manga)
    {
        await _mangaRepository.CreateAsync(manga);
    }

    public async Task<IEnumerable<Manga>> GetMangas()
    {
        return await _mangaRepository.GetAsync();
    }

    public async Task<IEnumerable<MangaUserDto>> GetMangasWithFilter(string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList)
    {
        var mangas = await _mangaRepository.GetWithFiltersAsync(orderBy, sourceIdList, genreIdList);

        return _mapper.Map<IEnumerable<MangaUserDto>>(mangas);
    }

    public async Task<Manga?> GetMangaById(int id)
    {
        return await _mangaRepository.GetByIdOrderedDescAsync(id);
    }

    public async Task<MangaDto?> GetMangaNotLoggedById(int id)
    {
        var data = await _mangaRepository.GetByIdOrderedDescAsync(id);

        return data == null ? null : _mapper.Map<MangaDto>(data);
    }

    public async Task<MangaDto?> GetMangaByIdAndUserId(int id, string userId)
    {
        var data = await _mangaRepository.GetByIdAndUserIdOrderedDescAsync(id, userId);

        return data == null ? null : _mapper.Map<MangaDto>(data);
    }

    public async Task<Manga?> GetMangaByMalId(int malId)
    {
        return await _mangaRepository.GetByMalIdAsync(malId);
    }
}