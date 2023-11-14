using System.Globalization;
using MangaUpdater.Application.Models.External;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class UserMangaRepository : BaseRepository<UserManga>, IUserMangaRepository
{
    public UserMangaRepository(IdentityMangaUpdaterContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserManga>> GetAllByUserIdAsync(string userId)
    {
        return await Get()
            .Where(um => um.UserId == userId)
            .Include(um => um.Manga)
            .ThenInclude(m => m!.MangaTitles)
            .Include(um => um.UserChapter)
            .ThenInclude(uc => uc!.Source)
            .GroupBy(um => um.MangaId)
            .Select(group => group.First())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserManga?> GetByMangaIdAndUserIdAsync(int mangaId, string userId)
    {
        return await Get()
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

    public async Task DeleteAsync(int mangaId, string userId)
    {
        var userMangas = await Get()
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .ToListAsync();

        Context.UserMangas.RemoveRange(userMangas);
        await Context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(UserManga userManga)
    {
        Context.Entry(userManga).Property(x => x.Id).IsModified = false;
        await Context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserManga>> GetMangasToUpdateChaptersAsync(string userId)
    {
        var result = await Get()
            .AsNoTracking()
            .Include(um => um.UserChapter)
            .ThenInclude(ms => ms.Source)
            .Include(um => um.Manga)
            .ThenInclude(m => m.Chapters)
            .Include(um => um.Manga)
            .ThenInclude(m => m.MangaSources)
            .Where(um => um.UserId == userId)
            .ToListAsync();
        
        result.ForEach(um =>
        {
            um.Manga.Chapters = um.Manga.Chapters
                .Where(ch => ch.MangaId == um.MangaId)
                .OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture))
                .Take(1)
                .ToList();
        });

        return result;
    }
}