using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

public class AuthController(IMediator mediator) : BaseController
{
    [AllowAnonymous]
    [SwaggerOperation("Register an user")]
    [HttpPost("register")]
    public async Task UserRegister([FromBody] RegisterUserCommand request)
    {
        await mediator.Send(request);
    }

    [AllowAnonymous]
    [SwaggerOperation("Authenticate an user")]
    [HttpPost("login")]
    public async Task<AuthenticateUserResponse> UserLogin([FromBody] AuthenticateUserQuery request)
    {
        return await mediator.Send(request);
    }
    
    [Authorize(Policy = "RefreshToken")]
    [SwaggerOperation("Refresh Token")]
    [HttpPost("refresh")]
    public async Task<RefreshTokenResponse> RefreshToken()
    {
        return await mediator.Send(new RefreshTokenQuery());
    }
}