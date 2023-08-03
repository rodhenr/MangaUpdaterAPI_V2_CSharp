using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddUser(User user)
    {
        await _userRepository.CreateAsync(user);

        return;
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
