using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Infra.Data.Repositories;
public class UserRepository : IUserRepository
{
    public Task CreateAsync(string name, string email, string avatar)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
