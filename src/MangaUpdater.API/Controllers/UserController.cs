using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Users;

namespace MangaUpdater.API.Controllers;

public class UserController(ISender mediator) : BaseController
{
    /// <summary>
    /// Get all followed manga for a logged-in user.
    /// </summary>
    /// <returns>All followed manga for a logged-in user, if any..</returns>
    /// <response code="200">Returns all followed manga, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga for a logged-in user")]
    [HttpGet("mangas")]
    public async Task<GetUserMangasResponse> GetUserMangasList()
    {
        return await mediator.Send(new GetUserMangasQuery());
    }

    /// <summary>
    /// Get all followed manga by an user.
    /// </summary>
    /// <returns>All followed manga for an user, if any..</returns>
    /// <response code="200">Returns all followed manga for an user, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga by a user")]
    [HttpGet("{userId}/mangas")]
    public async Task<GetUserMangasResponse> GetUserMangas([FromQuery] GetUserMangasQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// A logged-in user starts following sources from a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user starts following sources from a manga")]
    [HttpPost("mangas/{mangaId:int}")]
    public async Task<UpdateFollowedSourcesResponse> FollowSourcesFromManga([FromQuery] int mangaId, List<int> sourceIds)
    {
        return await mediator.Send(new UpdateFollowedSourcesQuery(mangaId, sourceIds));
    }

    /// <summary>
    /// A logged-in user no longer follows a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user no longer follows a manga")]
    [HttpDelete("mangas/{mangaId:int}")]
    public async Task<UnfollowMangaResponse> UnfollowManga([FromQuery] UnfollowMangaQuery request)
    {
        return await mediator.Send(request);
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
    /// Get all sources from a manga with following info for a logged-in user.
    /// </summary>
    /// <returns>All manga sources, if any.</returns>
    /// <response code="200">Returns all manga sources.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all sources from a manga with following info for a logged-in user")]
    [HttpGet("mangas/{mangaId:int}/sources")]
    public async Task<ActionResult<IEnumerable<UserSourceDto>>> GetUserSources(int mangaId)
    {
        var userManga = await _userMangaService.GetByUserIdAndMangaId(UserId!, mangaId);
        return Ok(await _userSourceService.GetUserSourcesByMangaId(mangaId, userManga?.Id));
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