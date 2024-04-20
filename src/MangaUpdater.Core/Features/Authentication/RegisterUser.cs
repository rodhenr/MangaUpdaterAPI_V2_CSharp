using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Models;
using MangaUpdater.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Features.Authentication;

public record RegisterUserCommand([FromBody] UserRegisterModel UserRegisterModel) : IRequest<RegisterUserResponse>;

public record RegisterUserResponse(UserRegisterResponseModel UserRegisterResponseModel);

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    
    public RegisterUserHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var createdUser = await CreateUser(request);
        var response = new RegisterUserResponse(new UserRegisterResponseModel(createdUser.Succeeded));
        
        if (createdUser.Succeeded || !createdUser.Errors.Any())return response;

        response.UserRegisterResponseModel.AddErrors(createdUser.Errors.Select(r => r.Description));
        throw new ValidationException(response.ToString());
    }

    private async Task<IdentityResult> CreateUser(RegisterUserCommand request)
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
        {
            await _userManager.SetLockoutEnabledAsync(appUser, false);
        }

        return result;
    }
}