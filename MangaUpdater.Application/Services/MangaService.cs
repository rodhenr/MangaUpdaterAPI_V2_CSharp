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

    public async Task Add(Manga manga)
    {
        _mangaRepository.CreateAsync(manga);
        await _mangaRepository.SaveAsync();
    }

    public async Task<IEnumerable<Manga>> Get()
    {
        return await _mangaRepository.GetAsync();
    }

    public async Task<Manga?> GetById(int id)
    {
        return await _mangaRepository.GetByIdOrderedDescAsync(id);
    }

    public async Task<IEnumerable<MangaUserDto>> GetWithFilter(string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList)
    {
        var mangas = await _mangaRepository.GetWithFiltersAsync(orderBy, sourceIdList, genreIdList);

        return _mapper.Map<IEnumerable<MangaUserDto>>(mangas);
    }

    public async Task<MangaDto?> GetByIdNotLogged(int id)
    {
        var data = await _mangaRepository.GetByIdOrderedDescAsync(id);

        return data == null ? null : _mapper.Map<MangaDto>(data);
    }

    public async Task<MangaDto?> GetByIdAndUserId(int id, string userId)
    {
        var data = await _mangaRepository.GetByIdAndUserIdOrderedDescAsync(id, userId);

        return _mapper.Map<MangaDto>(data);
    }
}