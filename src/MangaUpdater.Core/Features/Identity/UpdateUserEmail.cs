using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Services;
using MangaUpdater.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Features.Identity;

public record UpdateUserEmailCommand([FromBody] string Email) : IRequest;

public sealed class UpdateUserEmailHandler : IRequestHandler<UpdateUserEmailCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public UpdateUserEmailHandler(UserManager<AppUser> userManager, IMediator mediator, CurrentUserAccessor currentUserAccessor)
    {
        _userManager = userManager;
        _mediator = mediator;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_currentUserAccessor.UserId) ?? throw new UserNotFoundException("User not found");
        
        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
        var result = await _userManager.ChangeEmailAsync(user, request.Email, changeEmailToken);
        
        if (!result.Succeeded) throw new BadRequestException("Failed to update email");
    }
}