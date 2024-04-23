﻿using Microsoft.AspNetCore.Mvc;
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
    public async Task<List<GetUserMangasResponse>> GetUserMangasList([FromQuery] GetUserMangasQuery request)
    {
        return await mediator.Send(request);
    }

    /// <summary>
    /// Get all followed manga by an user.
    /// </summary>
    /// <returns>All followed manga for an user, if any..</returns>
    /// <response code="200">Returns all followed manga for an user, if any.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all followed manga by a user")]
    [HttpGet("{userId:int}/mangas")]
    public async Task<List<GetUserMangasResponse>> GetUserMangas([FromQuery] GetUserMangasQuery request)
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
    public async Task FollowSourcesFromManga([FromRoute] int mangaId, [FromBody] List<int> sourceIds)
    {
        await mediator.Send(new UpdateFollowedSourcesCommand(mangaId, sourceIds));
    }

    /// <summary>
    /// A logged-in user no longer follows a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user no longer follows a manga")]
    [HttpDelete("mangas/{mangaId:int}")]
    public async Task TaskUnfollowManga([FromQuery] UnfollowMangaCommand request)
    {
        await mediator.Send(request);
    }

    /// <summary>
    /// A logged-in user no longer follows a source from a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user no longer follows a source from a manga")]
    [HttpDelete("mangas/{mangaId:int}/sources/{sourceId:int}")]
    public async Task DeleteUserManga([FromQuery] DeleteSourceCommand request)
    {
        await mediator.Send(request);
    }
    
    /// <summary>
    /// A logged-in user changes its last chapter read from a combination of manga and source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("A logged-in user changes its last chapter read from a combination of manga and source")]
    [HttpPatch("mangas/{mangaId:int}/sources/{sourceId:int}")]
    public async Task UpdateManga([FromRoute] int mangaId, [FromRoute] int sourceId, [FromBody] UpdateChapterRequest requestBody)
    {
        await mediator.Send(new UpdateChapterCommand(mangaId, sourceId, requestBody.ChapterId));
    }
    
    /// <summary>
    /// Get all sources from a manga with following info for a logged-in user.
    /// </summary>
    /// <returns>All manga sources, if any.</returns>
    /// <response code="200">Returns all manga sources.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all sources from a manga with following info for a logged-in user")]
    [HttpGet("mangas/{mangaId:int}/sources")]
    public async Task<List<GetUserMangaSourcesResponse>> GetUserSources([FromQuery] GetUserMangaSourcesQuery request)
    {
        return await mediator.Send(request);
    }
}