using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task Add(Manga manga);
    Task<IEnumerable<Manga>> Get();
    Task<IEnumerable<MangaUserDto>> GetWithFilter(int page, string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList);
    Task<Manga> GetById(int id);
    Task<bool> CheckIfMangaIsRegistered(int myAnimeListId);
    Task<MangaDto> GetByIdNotLogged(int id);
    Task<MangaDto> GetByIdAndUserId(int id, string userId);
}