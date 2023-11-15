using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Application.Interfaces.Background;

namespace MangaUpdater.API.Controllers;

public class UserController : BaseController
{
    private string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserMangaService _userMangaService;
    private readonly IMangaService _mangaService;
    private readonly IHangfireService _hangfireService;

    public UserController(IUserMangaChapterService userMangaChapterService, IUserMangaService userMangaService, IMangaService mangaService, IHangfireService hangfireService)
    {
        _userMangaChapterService = userMangaChapterService;
        _userMangaService = userMangaService;
        _mangaService = mangaService;
        _hangfireService = hangfireService;
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
    public async Task<ActionResult<IEnumerable<MangaUserLoggedDto>>> GetLoggedUserMangas() =>
        Ok(await _userMangaChapterService.GetUserMangasWithThreeLastChapterByUserId(UserId!));

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
    /// Update all mangas followed by an user
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in request the update of his followed mangas")]
    [HttpGet("mangas/update")]
    public async Task<ActionResult> QueueMangasToUpdate()
    {
        await _hangfireService.AddHangfireJobs();
        RecurringJob.TriggerJob("JobForMangaId_4_SourceId_3");
        
        return Ok();
    }
}