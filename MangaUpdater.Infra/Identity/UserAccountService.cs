using System.Security.Authentication;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MangaUpdater.Infra.Data.Identity;

public class UserAccountService : IUserAccountService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public UserAccountService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<UserProfileDto> GetUserInfo(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AuthenticationException("User not found");

        return new UserProfileDto(user.Avatar, user.UserName!, user.Id, user.Email!);
    }

    public async Task<IdentityResult> ChangeUserEmailAsync(string userId, string newEmail, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AuthenticationException("User not found");
        
        var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, password);
        
        if (!passwordVerificationResult)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Password is incorrect." });
        }

        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        
        return await _userManager.ChangeEmailAsync(user, newEmail, changeEmailToken);
    }

    public async Task<IdentityResult> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AuthenticationException("User not found");

        var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, currentPassword);

        if (!passwordVerificationResult)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Current password is incorrect." });
        }

        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
}