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

public record UpdateUserEmailQuery([FromQuery] ChangeEmailModel Data) : IRequest<UpdateUserEmailResponse>;
public record UpdateUserEmailResponse;

public sealed class UpdateUserEmailHandler : IRequestHandler<UpdateUserEmailQuery, UpdateUserEmailResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public UpdateUserEmailHandler(UserManager<AppUser> userManager, CurrentUserAcessor currentUserAcessor)
    {
        _userManager = userManager;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<UpdateUserEmailResponse> Handle(UpdateUserEmailQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        if (!request.Data.Password.Equals(request.Data.ConfirmationPassword)) throw new BadRequestException("");
        
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user is null) throw new AuthenticationException("User not found");
        
        var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, request.Data.Password);
        
        if (!passwordVerificationResult) throw new AuthenticationException("Password is incorrect");

        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Data.NewEmail);
        
        var result = await _userManager.ChangeEmailAsync(user, request.Data.NewEmail, changeEmailToken);

        if (!result.Succeeded) throw new BadRequestException("");
        
        return new UpdateUserEmailResponse();
    }
}