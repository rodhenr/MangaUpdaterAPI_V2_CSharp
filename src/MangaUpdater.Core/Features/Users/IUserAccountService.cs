using MangaUpdater.Core.Dtos;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Application.Interfaces;

public interface IUserAccountService
{
    Task<UserProfileDto> GetUserInfo(string userId);
    Task<IdentityResult> ChangeUserEmailAsync(string userId, string newEmail, string password);
    Task<IdentityResult> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword);
}