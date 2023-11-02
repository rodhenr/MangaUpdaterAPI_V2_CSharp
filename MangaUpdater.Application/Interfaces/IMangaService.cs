using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task Add(Manga manga);
    Task<IEnumerable<MangaUserDto>> GetWithFilter(int page, int pageSize, string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList);
    Task<int> CheckNumberOfPages(int pageSize);
    Task<bool> CheckIfMangaIsRegistered(int myAnimeListId);
    Task<MangaDto> GetByIdNotLogged(int id);
    Task<MangaDto> GetByIdAndUserId(int id, string userId);
}