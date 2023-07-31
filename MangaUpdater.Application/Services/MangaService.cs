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

    public async Task<Manga?> GetMangaById(int id)
    {
        return await _mangaRepository.GetByIdAsync(id);
    }

    public async Task<MangaDTO> GetMangaByIdAndUserId(int id, int userId)
    {
        var data = await _mangaRepository.GetByIdAndUserIdAsync(id, userId);
        return _mapper.Map<MangaDTO>(data);
    }

    public async Task<IEnumerable<Manga>> GetMangas()
    {
        return await _mangaRepository.GetMangasAsync();
    }
}
