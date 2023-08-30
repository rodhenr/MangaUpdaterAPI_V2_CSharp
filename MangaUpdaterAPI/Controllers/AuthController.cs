using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models;
using MangaUpdater.API.Controllers.Shared;

namespace MangaUpdater.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthenticationService _identityService;

    public AuthController(IAuthenticationService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Register an user.
    /// </summary>
    /// <param name="userRegister">Data to register an user.</param>
    /// <response code="200">Returns success.</response>
    /// <response code="400">Returns all validation errors.</response>
    [SwaggerOperation("Register")]
    [HttpPost("register")]
    public async Task<ActionResult<UserRegisterResponse>> UserRegister(UserRegister userRegister)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var registered = await _identityService.Register(userRegister);

        if (registered.Success)
            return Ok(true);

        return BadRequest(registered.ErrorList);
    }

    /// <summary>
    /// Authenticate an user.
    /// </summary>
    /// <param name="userAuthenticate">Data to authenticate an user.</param>
    /// <response code="200">Returns token.</response>
    /// <response code="400">Returns all validation errors.</response>
    [SwaggerOperation("Login")]
    [HttpPost("login")]
    public async Task<ActionResult<UserAuthenticateResponse>> UserLogin(UserAuthenticate userAuthenticate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var response = await _identityService.Authenticate(userAuthenticate);

        if (!response.IsSuccess)
            return Unauthorized(response.ErrorList);

        return Ok(response);
    }
}