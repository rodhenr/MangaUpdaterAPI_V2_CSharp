using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Auth;

namespace MangaUpdater.API.Controllers;

[EnableCors]
public class AuthController(ISender mediator) : BaseController
{
    /// <summary>
    /// Register an user.
    /// </summary>
    /// <param name="userRegister">Data to register an user.</param>
    /// <response code="200">Returns success.</response>
    /// <response code="400">Returns all validation errors.</response>
    [AllowAnonymous]
    [SwaggerOperation("Register an user")]
    [HttpPost("register")]
    public async Task<RegisterUserResponse> UserRegister([FromQuery] RegisterUserQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// Authenticate an user.
    /// </summary>
    /// <param name="userAuthenticate">Data to authenticate an user.</param>
    /// <response code="200">Returns token.</response>
    /// <response code="400">Returns all validation errors.</response>
    [AllowAnonymous]
    [SwaggerOperation("Authenticate an user")]
    [HttpPost("login")]
    public async Task<AuthenticateUserResponse> UserLogin([FromQuery] AuthenticateUserQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// Refresh Token.
    /// </summary>
    /// <response code="200">Returns token.</response>
    /// <response code="400">Returns all validation errors.</response>
    [Authorize(Policy = "RefreshToken")]
    [SwaggerOperation("Refresh Token")]
    [HttpPost("refresh")]
    public async Task<AuthenticateUserResponse> RefreshToken([FromQuery] AuthenticateUserQuery request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Changes email for the logged user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Changes email for the logged user")]
    [HttpPost("profile/email")]
    public async Task<UpdateUserEmailResponse> UpdateUserEmail([FromQuery] UpdateUserEmailQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// Changes password for the logged user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Changes password for the logged user")]
    [HttpPost("profile/password")]
    public async Task<UpdateUserPasswordResponse> ChangeLoggedUserPassword([FromQuery] UpdateUserPasswordQuery request)
    {
        return await mediator.Send(request);
    }
}