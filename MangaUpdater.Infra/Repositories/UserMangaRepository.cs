﻿using MangaUpdater.Domain.Entities;
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

    public async Task<IEnumerable<UserManga>> GetAllByMangaIdAsync(int mangaId)
    {
        return await _context.UserMangas
            .Where(a => a.MangaId == mangaId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetAllByUserIdAsync(int userId)
    {
        return await _context.UserMangas
            .Where(a => a.UserId == userId)
            .Include(a => a.Manga)
            .Include(a => a.Source)
            .AsNoTracking()
            .GroupBy(a => a.MangaId)
            .Select(group => group.First())
            .ToListAsync();
    }

    public async Task<IEnumerable<UserManga>> GetAllByMangaIdAndUserIdAsync(int mangaId, int userId)
    {
        return await _context.UserMangas
            .Where(a => a.MangaId == mangaId && a.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserManga?> GetByMangaIdUserIdAndSourceIdAsync(int mangaId, int userId, int sourceId)
    {
        return await _context.UserMangas
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.UserId == userId && a.MangaId == mangaId && a.SourceId == sourceId);
    }

    public async Task UpdateAsync(int userId, int mangaId, int sourceId, int chapterId)
    {
        var userManga = await _context.UserMangas
            .SingleOrDefaultAsync(a => a.UserId == userId && a.MangaId == mangaId && a.SourceId == sourceId);

        if (userManga == null)
        {
            throw new Exception("User doesn't follow this manga/source");
        }

        userManga.CurrentChapterId = chapterId;
        await _context.SaveChangesAsync();

        return;
    }

    public async Task DeleteAsync(int userId, int mangaId, int sourceId)
    {
        var userManga = await _context.UserMangas.SingleOrDefaultAsync(a => a.UserId == userId && a.MangaId == mangaId && a.SourceId == sourceId);

        if (userManga != null)
        {
            _context.UserMangas.Remove(userManga);
            await _context.SaveChangesAsync();
        }

        return;
    }

    public async Task DeleteAllByMangaIdAndUserIdAsync(int mangaId, int UserId)
    {
        var userMangas = await _context.UserMangas
            .Where(a => a.MangaId == mangaId && a.UserId == UserId)
            .ToListAsync();

        _context.UserMangas.RemoveRange(userMangas);

        await _context.SaveChangesAsync();

        return;
    }
}