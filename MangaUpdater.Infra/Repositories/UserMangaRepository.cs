using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class UserMangaRepository : IUserMangaRepository
{
    private readonly MangaUpdaterContext _context;

    public UserMangaRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(UserManga userManga)
    {
        await _context.UserMangas.AddAsync(userManga);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task<IEnumerable<UserManga>> GetByMangaIdAndUserIdAsync(int mangaId, int userId)
    {
        return await _context.UserMangas
            .Where(a => a.MangaId == mangaId && a.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetByMangaIdAsync(int mangaId)
    {
        return await _context.UserMangas
            .Where(a => a.MangaId == mangaId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetByUserIdAsync(int userId)
    {
        return await _context.UserMangas
            .Where(a => a.UserId == userId)
            .Include(a => a.Manga)
            .ThenInclude(a => a.Chapters
                .OrderByDescending(b => b.Number)
                .Take(3)
            )
            .ToListAsync();
    }

    public async Task UpdateAsync(int userId, int mangaId, int sourceId, int chapterId)
    {
        var userManga = await _context.UserMangas
            .SingleOrDefaultAsync(a => a.UserId == userId && a.MangaId == mangaId && a.SourceId == sourceId);

        if(userManga == null)
        {
            throw new Exception("Not found");
        }

        userManga.CurrentChapterId = chapterId;
        await _context.SaveChangesAsync();

        return;
    }
}
