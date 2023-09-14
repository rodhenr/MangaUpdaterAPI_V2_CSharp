using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaTitleService
{
    Task<IEnumerable<MangaTitle>> GetAllByMangaId(int mangaId);
}