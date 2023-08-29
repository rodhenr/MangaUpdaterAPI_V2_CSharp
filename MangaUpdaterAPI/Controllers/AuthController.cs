using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _identityService;

    public AuthController(IAuthenticationService identityService)
    {
        _identityService = identityService;
    }

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