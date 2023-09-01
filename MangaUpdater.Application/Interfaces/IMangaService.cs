using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task Add(Manga manga);
    Task<IEnumerable<Manga>> Get();

    Task<IEnumerable<MangaUserDto>> GetWithFilter(string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList);

    Task<Manga?> GetById(int id);
    Task<MangaDto?> GetByIdNotLogged(int id);
    Task<MangaDto?> GetByIdAndUserId(int id, string userId);
}