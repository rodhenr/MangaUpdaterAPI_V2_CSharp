using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class UserChapterRepository : BaseRepository<UserChapter>, IUserChapterRepository
{
    public UserChapterRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserChapter>> GetByUserMangaIdAsync(int userMangaId)
    {
        return await Get()
            .Where(uc => uc.UserMangaId == userMangaId)
            .ToListAsync();
    }

    public async Task<UserChapter?> GetByUserMangaIdAndSourceIdAsync(int userMangaId, int sourceId)
    {
        return await Get()
            .Where(uc => uc.UserMangaId == userMangaId && uc.SourceId == sourceId)
            .SingleOrDefaultAsync();
    }

    public async Task DeleteAsync(int userMangaId)
    {
        var userChapters = await Get()
            .Where(uc => uc.UserMangaId == userMangaId)
            .ToListAsync();

        Context.UserChapters.RemoveRange(userChapters);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userMangaId, int sourceId)
    {
        var userChapters = await Get()
            .Where(uc => uc.UserMangaId == userMangaId && uc.SourceId == sourceId)
            .ToListAsync();

        Context.UserChapters.RemoveRange(userChapters);
        await Context.SaveChangesAsync();
    }
}