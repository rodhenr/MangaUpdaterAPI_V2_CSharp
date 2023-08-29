using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task AddManga(Manga manga);
    Task<IEnumerable<Manga>> GetMangas();

    Task<IEnumerable<MangaUserDto>> GetMangasWithFilter(string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList);

    Task<Manga?> GetMangaById(int id);
    Task<MangaDto?> GetMangaNotLoggedById(int id);
    Task<Manga?> GetMangaByMalId(int malId);
    Task<MangaDto?> GetMangaByIdAndUserId(int id, string userId);
}