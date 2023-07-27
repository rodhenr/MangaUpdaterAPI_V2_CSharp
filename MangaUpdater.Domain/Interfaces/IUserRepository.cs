using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> GetByIdAsync(int id);
}
