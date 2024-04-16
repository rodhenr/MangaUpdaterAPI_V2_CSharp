using MangaUpdater.Core.Dtos;
using MangaUpdater.Data.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaSourceService
{
    void Add(MangaSource mangaSource);
    Task<MangaSource> GetByMangaIdAndSourceId(int mangaId, int sourceId);
    Task SaveChanges();
}