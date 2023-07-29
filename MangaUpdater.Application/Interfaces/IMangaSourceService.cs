using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaSourceService
{
    Task<IEnumerable<MangaSource>> GetAllByMangaId(int mangaId);
}
