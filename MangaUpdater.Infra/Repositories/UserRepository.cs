using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MangaUpdaterContext _context;

    public UserRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.SingleOrDefaultAsync(a => a.Id == id);
    }
}
