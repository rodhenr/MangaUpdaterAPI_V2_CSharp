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

    public async Task<IEnumerable<MangaUserDTO>> GetMangasWithFilter(string? orderBy, List<int>? sourceIdList, List<int>? genreIdList)
    {
        var mangas = await _mangaRepository.GetWithFiltersAsync(orderBy, sourceIdList, genreIdList);

        return _mapper.Map<IEnumerable<MangaUserDTO>>(mangas);
    }

    public async Task<IEnumerable<MangaUserDTO>> GetMangasByUserId(int userId)
    {
        IEnumerable<Manga> mangas = await _mangaRepository.GetAllByUserId(userId);

        return _mapper.Map<IEnumerable<MangaUserDTO>>(mangas);
    }

    public async Task<Manga?> GetMangaById(int id)
    {
        return await _mangaRepository.GetByIdOrderedDescAsync(id);
    }

    public async Task<MangaDTO?> GetMangaByIdAndUserId(int id, int userId)
    {
        var data = await _mangaRepository.GetByIdAndUserIdOrderedDescAsync(id, userId);

        if (data == null)
        {
            return null;
        }

        return _mapper.Map<MangaDTO>(data);
    }

    public async Task<Manga?> GetMangaByMalId(int malId)
    {
        return await _mangaRepository.GetByMalIdAsync(malId);
    }

    public async Task<IEnumerable<MangaUserLoggedDTO>> GetMangasByUserIdLogged(int userId)
    {
        IEnumerable<Manga> mangas = await _mangaRepository.GetAllByUserLoggedIdWithLastThreeChapters(userId);

        return _mapper.Map<IEnumerable<MangaUserLoggedDTO>>(mangas);
    }
}
