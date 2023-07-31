using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task AddManga(Manga manga);
    Task<Manga?> GetMangaById(int id);
    Task<MangaDTO> GetMangaByIdAndUserId(int id, int userId);
    Task<IEnumerable<Manga>> GetMangas();
}
