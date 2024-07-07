using MangaUpdater.Controllers.Shared;
using MangaUpdater.Features.Auth.Commands;
using MangaUpdater.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.Controllers;

public class AuthController(ISender mediator) : BaseController
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