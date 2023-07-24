using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateAsync(string name, string email, string avatar);
    Task<User> GetByIdAsync(int id);
}
