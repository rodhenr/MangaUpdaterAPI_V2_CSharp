using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaTitleRepository: IBaseRepository<MangaTitle>
{
    void BulkCreateAsync(IEnumerable<MangaTitle> mangaTitles);
    Task<IEnumerable<MangaTitle>> GetByMangaIdAsync(int mangaId);
}