using AutoMapper;
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

    public async Task<IEnumerable<UserManga>> GetUserMangasByMangaIdAndUserId(int mangaId, int userId)
    {
        return await _userMangaRepository.GetAllByMangaIdAndUserIdAsync(mangaId, userId);
    }

    public async Task<IEnumerable<UserManga>> GetUserMangasByMangaId(int mangaId)
    {
        return await _userMangaRepository.GetAllByMangaIdAsync(mangaId);
    }

    public async Task UpdateUserMangaAsync(int userId, int mangaId, int sourceId, int chapterId)
    {
        await _userMangaRepository.UpdateAsync(userId,mangaId,sourceId,chapterId);

        return;
    }
}
