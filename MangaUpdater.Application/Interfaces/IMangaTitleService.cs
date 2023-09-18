using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaTitleService
{
    void BulkCreate(IEnumerable<MangaTitle> mangaTitles);
    Task<IEnumerable<MangaTitle>> GetAllByMangaId(int mangaId);
}