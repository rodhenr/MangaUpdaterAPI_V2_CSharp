using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Models;
using MangaUpdater.Data.Entities;

namespace MangaUpdater.Core.Features.Auth;

public record RegisterUserQuery([FromQuery] UserRegisterModel UserRegisterModel) : IRequest<RegisterUserResponse>;
public record RegisterUserResponse(UserRegisterResponseModel UserRegisterResponseModel);

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserQuery, RegisterUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    
    public RegisterUserHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserQuery request, CancellationToken cancellationToken)
    {
        var appUser = new AppUser
        {
            UserName = request.UserRegisterModel.UserName,
            Email = request.UserRegisterModel.Email,
            EmailConfirmed = true,
            Avatar = ""
        };

        var result = await _userManager.CreateAsync(appUser, request.UserRegisterModel.Password);

        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(appUser, false);

        var userResponse = new RegisterUserResponse(new UserRegisterResponseModel(result.Succeeded));

        if (result.Succeeded || !result.Errors.Any()) return userResponse;

        userResponse.UserRegisterResponseModel.AddErrors(result.Errors.Select(r => r.Description));
        throw new ValidationException(userResponse.ToString());
    }
}