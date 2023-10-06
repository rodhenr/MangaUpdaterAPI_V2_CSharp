using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangaTitleRepository: IBaseRepository<MangaTitle>
{
    void BulkCreate(IEnumerable<MangaTitle> mangaTitles);
}