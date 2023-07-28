using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task AddManga(Manga manga);
    Task<MangaDTO> GetMangaById(int id);
    Task<IEnumerable<Manga>> GetMangas();
}
