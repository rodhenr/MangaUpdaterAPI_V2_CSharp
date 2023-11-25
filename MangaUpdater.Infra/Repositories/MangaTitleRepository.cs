using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaTitleRepository : BaseRepository<MangaTitle>, IMangaTitleRepository
{
    public MangaTitleRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public void BulkCreate(IEnumerable<MangaTitle> mangaTitles)
    {
        Context.MangaTitles.AddRange(mangaTitles);
    }
}