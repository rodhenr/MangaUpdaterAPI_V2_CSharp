using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly MangaUpdaterContext _context;

    public MangaRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Manga manga)
    {
        await _context.AddAsync(manga);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task<IEnumerable<Manga>> GetAsync()
    {
        return await _context.Mangas
            .ToListAsync();
    }

    public async Task<Manga?> GetByIdOrderedDescAsync(int id)
    {
        return await _context.Mangas
            .Include(a => a.MangaGenres)
                .ThenInclude(a => a.Genre)
            .Include(a => a.MangaSources)
                .ThenInclude(a => a.Source)
            .Include(a => a.Chapters.OrderByDescending(b => b.Date))
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Manga?> GetByIdAndUserIdOrderedDescAsync(int id, int userId)
    {
        return await _context.Mangas
            .Include(a => a.UserMangas.Where(b => b.UserId == userId))
            .Include(a => a.MangaGenres)
                .ThenInclude(a => a.Genre)
            .Include(a => a.MangaSources)
                .ThenInclude(a => a.Source)
            .Include(a => a.Chapters.OrderByDescending(b => b.Date))
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Manga>> GetAllByUserLoggedIdWithLastThreeChapters(int userId)
    {
        return await _context.Mangas
            .Include(a => a.UserMangas
                .Where(b => b.UserId == userId))
            .Include(a => a.Chapters
                .OrderByDescending(b => b.Date).Take(3))
                    .ThenInclude(a => a.Source)
            .ToListAsync();
    }

    public async Task<IEnumerable<Manga>> GetAllByUserId(int userId)
    {
        return await _context.Mangas
            .Include(a => a.UserMangas
                .Where(b => b.UserId == userId))
            .ToListAsync();
    }
}
