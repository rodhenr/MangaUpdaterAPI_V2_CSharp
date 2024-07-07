using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Features.Auth.Commands;

public record RegisterUserCommand([FromBody] string UserName, [FromBody] string Email, [FromBody] string Password, [FromBody] string ConfirmationPassword) : IRequest;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    
    public RegisterUserHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var createdUser = await CreateUser(request);
        
        if (createdUser.Errors.Any()) throw new AuthorizationException(createdUser.Errors.First().Description);
    }

    private async Task<IdentityResult> CreateUser(RegisterUserCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            throw new AuthorizationException("Email already taken");
        }
        
        var appUser = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            EmailConfirmed = true,
            Avatar = ""
        };
        
        var result = await _userManager.CreateAsync(appUser, request.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description ?? "Failed to register");
        }
        
        await _userManager.SetLockoutEnabledAsync(appUser, false);
        
        return result;
    }
}