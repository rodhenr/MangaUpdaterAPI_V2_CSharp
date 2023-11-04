using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task Add(Manga manga);

    Task<MangaDataWithPagesDto> GetWithFilter(int page, int pageSize, string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList, string? input);

    Task<bool> CheckIfMangaIsRegistered(int myAnimeListId);
    Task<MangaDataWithHighlightedMangasDto> GetByIdNotLogged(int id, int quantity);
    Task<MangaDataWithHighlightedMangasDto> GetByIdAndUserId(int id, string userId, int quantity);
}