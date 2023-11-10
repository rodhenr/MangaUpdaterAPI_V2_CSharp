using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserMangaService : IUserMangaService
{
    private readonly IUserMangaRepository _userMangaRepository;
    private readonly IMapper _mapper;

    public UserMangaService(IMapper mapper, IUserMangaRepository userMangaRepository)
    {
        _userMangaRepository = userMangaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MangaUserDto>> GetMangasByUserId(string userId)
    {
        var mangas = await _userMangaRepository.GetAllByUserIdAsync(userId);

        return _mapper.Map<IEnumerable<MangaUserDto>>(mangas.Select(userManga => userManga.Manga));
    }

    public async Task<UserManga?> GetByUserIdAndMangaId(string userId, int mangaId)
    {
        return await _userMangaRepository.GetByMangaIdAndUserIdAsync(mangaId, userId);
    }

    public async Task Update(UserManga userManga)
    {
        _userMangaRepository.Update(userManga);
        await _userMangaRepository.SaveChangesAsync(userManga);
    }
}