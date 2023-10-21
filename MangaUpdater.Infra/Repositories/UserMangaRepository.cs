﻿using Microsoft.EntityFrameworkCore;
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
            .ThenInclude(m => m.MangaTitles)
            .Include(um => um.Source)
            .GroupBy(um => um.MangaId)
            .Select(group => group.First())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetAllByMangaIdAndUserIdAsync(int mangaId, string userId)
    {
        return await Get()
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserManga?> GetByMangaIdUserIdAndSourceIdAsync(int mangaId, string userId, int sourceId)
    {
        return await Get()
            .SingleOrDefaultAsync(um => um.UserId == userId && um.MangaId == mangaId && um.SourceId == sourceId);
    }

    public async Task DeleteAsync(int mangaId, string userId)
    {
        var userMangas = await Get()
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .ToListAsync();

        Context.UserMangas.RemoveRange(userMangas);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int mangaId, int sourceId, string userId)
    {
        var userMangas = await Get()
            .Where(um => um.MangaId == mangaId && um.SourceId == sourceId && um.UserId == userId)
            .ToListAsync();

        Context.UserMangas.RemoveRange(userMangas);
        await Context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(UserManga userManga)
    {
        Context.Entry(userManga).Property(x => x.Id).IsModified = false;
        await Context.SaveChangesAsync();
    }
}