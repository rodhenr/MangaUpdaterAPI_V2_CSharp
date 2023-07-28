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

    public Task<IEnumerable<UserManga>> GetByMangaIdAsync(int mangaId)
    {
        throw new NotImplementedException();
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
}
