using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

[EnableCors]
public class AuthController(ISender mediator) : BaseController
{
    /// <summary>
    /// Register an user.
    /// </summary>
    /// <param name="request">Data to register an user.</param>
    /// <response code="200">Returns success.</response>
    /// <response code="400">Returns all validation errors.</response>
    [AllowAnonymous]
    [SwaggerOperation("Register an user")]
    [HttpPost("register")]
    public async Task<RegisterUserResponse> UserRegister([FromBody] RegisterUserCommand request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// Authenticate an user.
    /// </summary>
    /// <param name="request">Data to authenticate an user.</param>
    /// <response code="200">Returns token.</response>
    /// <response code="400">Returns all validation errors.</response>
    [AllowAnonymous]
    [SwaggerOperation("Authenticate an user")]
    [HttpPost("login")]
    public async Task<AuthenticateUserResponse> UserLogin([FromBody] AuthenticateUserQuery request)
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
    public async Task<AuthenticateUserResponse> RefreshToken([FromBody] AuthenticateUserQuery request)
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
    public async Task<ActionResult> UpdateUserEmail([FromBody] UpdateUserEmailCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    /// <summary>
    /// Changes password for the logged user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Changes password for the logged user")]
    [HttpPost("profile/password")]
    public async Task<ActionResult> ChangeLoggedUserPassword([FromBody] UpdateUserPasswordCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }
}