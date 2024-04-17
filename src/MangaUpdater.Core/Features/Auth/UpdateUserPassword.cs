using System.Security.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Models;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Core.Features.Auth;

public record UpdateUserPasswordQuery([FromQuery] string Password, [FromQuery] string OldPassword) : IRequest<UpdateUserPasswordResponse>;
public record UpdateUserPasswordResponse;

public sealed class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordQuery, UpdateUserPasswordResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public UpdateUserPasswordHandler(UserManager<AppUser> userManager, CurrentUserAcessor currentUserAcessor)
    {
        _userManager = userManager;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<UpdateUserPasswordResponse> Handle(UpdateUserPasswordQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AuthenticationException("User not found");

        var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, request.OldPassword);

        if (!passwordVerificationResult) throw new AuthenticationException("Current password is incorrect.");

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

        if (!result.Succeeded) throw new BadRequestException("");
        
        
        return new UpdateUserPasswordResponse();
    }
}