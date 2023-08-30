using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class UserMangaRepository : IUserMangaRepository
{
    private readonly IdentityMangaUpdaterContext _context;

    public UserMangaRepository(IdentityMangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(UserManga userManga)
    {
        await _context.UserMangas.AddAsync(userManga);
        await _context.SaveChangesAsync();
    }

    public Task BulkCreateAsync(IEnumerable<UserManga> entities)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserManga>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserManga?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(UserManga entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<UserManga>> GetAllByMangaIdAsync(int mangaId)
    {
        return await _context.UserMangas
            .Where(um => um.MangaId == mangaId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetAllByUserIdAsync(string userId)
    {
        return await _context.UserMangas
            .Where(um => um.UserId == userId)
            .Include(um => um.Manga)
            .Include(um => um.Source)
            .AsNoTracking()
            .GroupBy(um => um.MangaId)
            .Select(group => group.First())
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetAllByMangaIdAndUserIdAsync(int mangaId, string userId)
    {
        return await _context.UserMangas
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserManga?> GetByMangaIdUserIdAndSourceIdAsync(int mangaId, string userId, int sourceId)
    {
        return await _context.UserMangas
            .AsNoTracking()
            .SingleOrDefaultAsync(um => um.UserId == userId && um.MangaId == mangaId && um.SourceId == sourceId);
    }

    public async Task UpdateAsync(string userId, int mangaId, int sourceId, int chapterId)
    {
        var userManga = await _context.UserMangas
            .SingleOrDefaultAsync(um => um.UserId == userId && um.MangaId == mangaId && um.SourceId == sourceId);

        if (userManga != null)
        {
            userManga.CurrentChapterId = chapterId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(string userId, int mangaId, int sourceId)
    {
        var userManga = await _context.UserMangas
            .SingleOrDefaultAsync(um => um.UserId == userId && um.MangaId == mangaId && um.SourceId == sourceId);

        if (userManga != null)
        {
            _context.UserMangas.Remove(userManga);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllByMangaIdAndUserIdAsync(int mangaId, string userId)
    {
        var userMangas = await _context.UserMangas
            .Where(um => um.MangaId == mangaId && um.UserId == userId)
            .ToListAsync();

        _context.UserMangas.RemoveRange(userMangas);

        await _context.SaveChangesAsync();
    }
}