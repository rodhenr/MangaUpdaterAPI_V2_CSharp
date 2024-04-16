using MangaUpdater.Data.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Data;

namespace MangaUpdater.Data.Repositories;

public class MangaTitleRepository : BaseRepository<MangaTitle>, IMangaTitleRepository
{
    public MangaTitleRepository(AppDbContextIdentity context) : base(context)
    {
    }

    public void BulkCreate(IEnumerable<MangaTitle> mangaTitles)
    {
        Context.MangaTitles.AddRange(mangaTitles);
    }
}