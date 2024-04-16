using MangaUpdater.Data.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaTitleService
{
    void BulkCreate(IEnumerable<MangaTitle> mangaTitles);
}