using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Application.Models;

namespace MangaUpdater.API.Controllers;

public class UserController : BaseController
{
    private string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserMangaService _userMangaService;
    private readonly IUserAccountService _userAccountService;

    public UserController(IUserMangaChapterService userMangaChapterService, IUserMangaService userMangaService,
        IUserAccountService userAccountService)
    {
        _userMangaChapterService = userMangaChapterService;
        _userMangaService = userMangaService;
        _userAccountService = userAccountService;
    }

    /// <summary>
    /// Get all followed manga for a logged-in user with 3 last released chapters from the sources followed.
    /// </summary>
    /// <returns>All followed manga for a logged-in user, if any..</returns>
    /// <response code="200">Returns all followed manga, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation(
        "Get all followed manga for a logged-in user with 3 last released chapters from the sources followed")]
    [HttpGet("mangas")]
    public async Task<ActionResult<IEnumerable<MangaUserLoggedDto>>> GetLoggedUserMangas([FromQuery] int page = 1,
        [FromQuery] int limit = 20) =>
        Ok(await _userMangaChapterService.GetUserMangasWithThreeLastChapterByUserId(UserId!, page, limit));

    /// <summary>
    /// Get all followed manga for a logged-in user.
    /// </summary>
    /// <returns>All followed manga for a logged-in user, if any..</returns>
    /// <response code="200">Returns all followed manga, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga for a logged-in user")]
    [HttpGet("mangas/list")]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetUserMangasList() =>
        Ok(await _userMangaService.GetMangasByUserId(UserId!));

    /// <summary>
    /// Get all followed manga by an user.
    /// </summary>
    /// <returns>All followed manga for an user, if any..</returns>
    /// <response code="200">Returns all followed manga for an user, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga by a user")]
    [HttpGet("{userId}/mangas")]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetUserMangas(string userId) =>
        Ok(await _userMangaService.GetMangasByUserId(userId));

    /// <summary>
    /// A logged-in user starts following sources from a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user starts following sources from a manga")]
    [HttpPost("mangas/{mangaId:int}")]
    public async Task<ActionResult> FollowSourcesFromManga(int mangaId, IEnumerable<int> sourceIdList)
    {
        await _userMangaChapterService.AddUserMangaBySourceIdList(mangaId, UserId!, sourceIdList);
        return Ok();
    }

    /// <summary>
    /// A logged-in user no longer follows a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user no longer follows a manga")]
    [HttpDelete("mangas/{mangaId:int}")]
    public async Task<ActionResult> UnfollowManga(int mangaId)
    {
        await _userMangaChapterService.DeleteUserMangasByMangaId(mangaId, UserId!);
        return Ok();
    }

    /// <summary>
    /// A logged-in user no longer follows a source from a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user no longer follows a source from a manga")]
    [HttpDelete("mangas/{mangaId:int}/sources/{sourceId:int}")]
    public async Task<ActionResult> DeleteUserManga(int mangaId, int sourceId)
    {
        await _userMangaChapterService.DeleteUserMangaByMangaIdAndSourceId(mangaId, sourceId, UserId!);
        return Ok();
    }

    /// <summary>
    /// A logged-in user changes its last chapter read from a combination of manga and source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user changes its last chapter read from a combination of manga and source")]
    [HttpPatch("mangas/{mangaId:int}/sources/{sourceId:int}")]
    public async Task<ActionResult> UpdateManga(int mangaId, int sourceId, [FromQuery] int chapterId)
    {
        await _userMangaChapterService.UpdateOrCreateUserChapter(UserId!, mangaId, sourceId, chapterId);
        return Ok();
    }

    /// <summary>
    /// Get information about the logged user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get information about the logged user")]
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetLoggedUserInfo() =>
        Ok(await _userAccountService.GetUserInfo(UserId!));

    /// <summary>
    /// Changes email for the logged user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Changes email for the logged user")]
    [HttpPost("profile/email")]
    public async Task<ActionResult> ChangeLoggedUserEmail(ChangeEmailQuery data)
    {
        if (!data.password.Equals(data.confirmationPassword)) return BadRequest();
        
        var result = await _userAccountService.ChangeUserEmailAsync(UserId!, data.newEmail, data.password);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
    }

    /// <summary>
    /// Changes password for the logged user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Changes password for the logged user")]
    [HttpPost("profile/password")]
    public async Task<ActionResult> ChangeLoggedUserPassword(string password, string oldPassword)
    {
        var result = await _userAccountService.ChangeUserPasswordAsync(UserId!, password, oldPassword);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
    }
}