using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using MangaUpdater.API.Controllers.Shared;

namespace MangaUpdater.API.Controllers;

[EnableCors]
public class AuthController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Register an user.
    /// </summary>
    /// <param name="userRegister">Data to register an user.</param>
    /// <response code="200">Returns success.</response>
    /// <response code="400">Returns all validation errors.</response>
    [AllowAnonymous]
    [SwaggerOperation("Register an user")]
    [HttpPost("register")]
    public async Task<ActionResult<UserRegisterResponse>> UserRegister(UserRegister userRegister) =>
        Ok(await _identityService.Register(userRegister));

    /// <summary>
    /// Authenticate an user.
    /// </summary>
    /// <param name="userAuthenticate">Data to authenticate an user.</param>
    /// <response code="200">Returns token.</response>
    /// <response code="400">Returns all validation errors.</response>
    [AllowAnonymous]
    [SwaggerOperation("Authenticate an user")]
    [HttpPost("login")]
    public async Task<ActionResult<UserAuthenticateResponse>> UserLogin(UserAuthenticate userAuthenticate) =>
        Ok(await _identityService.Authenticate(userAuthenticate));

    /// <summary>
    /// Refresh Token.
    /// </summary>
    /// <response code="200">Returns token.</response>
    /// <response code="400">Returns all validation errors.</response>
    [Authorize(Policy = "RefreshToken")]
    [SwaggerOperation("Refresh Token")]
    [HttpPost("refresh")]
    public async Task<ActionResult<UserAuthenticateResponse>> RefreshToken()
    {
        var tokensData = await _identityService.RefreshToken(UserId!);

        if (tokensData.IsSuccess) return Ok(tokensData);

        return Unauthorized();
    }
}