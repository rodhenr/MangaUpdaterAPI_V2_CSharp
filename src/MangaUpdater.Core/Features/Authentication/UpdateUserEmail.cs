using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Features.Authentication;

public record UpdateUserEmailCommand([FromBody] string Email, [FromBody] string Password, [FromBody] string ConfirmationPassword) : IRequest;

public sealed class UpdateUserEmailHandler : IRequestHandler<UpdateUserEmailCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;
    
    public UpdateUserEmailHandler(UserManager<AppUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetAndVerifyUserQuery(request.Password), cancellationToken);
        
        var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user.User, request.Email);
        var result = await _userManager.ChangeEmailAsync(user.User, request.Email, changeEmailToken);
        
        if (!result.Succeeded) throw new BadRequestException("Failed to update email");
    }
}