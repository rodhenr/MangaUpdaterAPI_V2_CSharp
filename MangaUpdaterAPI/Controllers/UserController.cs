using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.API.Controllers.Shared;

namespace MangaUpdater.API.Controllers;

public class UserController : BaseController
{
    private string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserSourceService _userSourceService;
    private readonly IUserMangaService _userMangaService;

    public UserController(IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService,
        IUserMangaService userMangaService)
    {
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
        _userMangaService = userMangaService;
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
    public async Task<ActionResult<IEnumerable<MangaUserLoggedDto>>> GetLoggedUserMangas()
    {
        return Ok(await _userMangaChapterService.GetUserMangasWithThreeLastChapterByUserId(UserId!));
    }

    /// <summary>
    /// Get all followed manga for a logged-in user.
    /// </summary>
    /// <returns>All followed manga for a logged-in user, if any..</returns>
    /// <response code="200">Returns all followed manga, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga for a logged-in user")]
    [HttpGet("mangas/list")]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetUserMangasList()
    {
        return Ok(await _userMangaService.GetMangasByUserId(UserId!));
    }

    /// <summary>
    /// Get all followed manga by an user.
    /// </summary>
    /// <returns>All followed manga for an user, if any..</returns>
    /// <response code="200">Returns all followed manga for an user, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga by a user")]
    [HttpGet("{userId}/mangas")]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetUserManga(string userId)
    {
        return Ok(await _userMangaService.GetMangasByUserId(userId));
    }

    /// <summary>
    /// A logged-in user starts following a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user starts following a manga")]
    [HttpPost("mangas/{mangaId:int}")]
    public async Task<ActionResult> FollowManga(int mangaId, IEnumerable<int> sourceIdList)
    {
        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, UserId!);
        await _userMangaChapterService.AddUserMangaBySourceIdList(mangaId, UserId!, sourceIdList, userSources);

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
    /// A logged-in user starts following a source from a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user starts following a source from a manga")]
    [HttpPost("mangas/{mangaId:int}/sources/{sourceId:int}")]
    public async Task<ActionResult> AddUserManga(int mangaId, int sourceId)
    {
        await _userMangaChapterService.AddUserManga(mangaId, UserId!, sourceId);
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
        await _userMangaChapterService.DeleteUserManga(mangaId, UserId!, sourceId);
        return Ok();
    }

    /// <summary>
    /// A logged-in user changes its last chapter read from a combination of manga and source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user changes its last chapter read from a combination of manga and source")]
    [HttpPatch("mangas/{mangaId:int}/sources/{sourceId:int}")]
    public async Task<ActionResult> UpdateManga(int mangaId, int sourceId, int chapterId)
    {
        var userManga = await _userMangaService.GetByMangaIdUserIdAndSourceId(mangaId, UserId!, sourceId);
        userManga.CurrentChapterId = chapterId;

        await _userMangaService.Update(userManga);
        return Ok();
    }
}