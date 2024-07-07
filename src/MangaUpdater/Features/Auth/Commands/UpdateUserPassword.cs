using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Features.Auth.Commands;

public record UpdateUserPasswordCommand([FromBody] string Password, [FromQuery] string OldPassword) : IRequest;

public sealed class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;
    
    public UpdateUserPasswordHandler(UserManager<AppUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetAndVerifyUserQuery(request.OldPassword), cancellationToken);

        var result = await _userManager.ChangePasswordAsync(user.User, request.OldPassword, request.Password);

        if (!result.Succeeded) throw new BadRequestException("Failed to update password");
    }
}