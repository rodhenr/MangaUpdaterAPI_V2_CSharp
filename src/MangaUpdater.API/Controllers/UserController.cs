﻿using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Identity;
using MangaUpdater.Core.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

public class UserController(IMediator mediator) : BaseController
{
    [SwaggerOperation("Get all followed manga for a logged-in user")]
    [HttpGet("manga")]
    public async Task<List<GetUserMangasResponse>> GetUserMangasList([FromQuery] GetUserMangasQuery request)
    {
        return await mediator.Send(request);
    }

    [SwaggerOperation("Get all followed manga by a user")]
    [HttpGet("{userId}/manga")]
    public async Task<List<GetUserMangasResponse>> GetUserMangas([FromQuery] GetUserMangasQuery request)
    {
        return await mediator.Send(request);
    }
    
    [SwaggerOperation("A logged-in user starts following a manga")]
    [HttpPost("manga/{mangaId:int}")]
    public async Task FollowManga([FromQuery] FollowMangaCommand request)
    {
        await mediator.Send(request);
    }

    [SwaggerOperation("A logged-in user no longer follows a manga")]
    [HttpDelete("manga/{mangaId:int}")]
    public async Task UnfollowManga([FromQuery] UnfollowMangaCommand request)
    {
        await mediator.Send(request);
    }
    
    [SwaggerOperation("Get all sources from a manga with following info for a logged-in user")]
    [HttpGet("manga/{mangaId:int}/source")]
    public async Task<List<GetUserMangaSourcesResponse>> GetUserSources([FromQuery] GetUserMangaSourcesQuery request)
    {
        return await mediator.Send(request);
    }
    
    [SwaggerOperation("A logged-in user starts following sources from a manga")]
    [HttpPost("manga/{mangaId:int}/source")]
    public async Task FollowSourcesFromManga([FromRoute] int mangaId, [FromBody] List<int> sourceIds)
    {
        await mediator.Send(new UpdateFollowedSourcesCommand(mangaId, sourceIds));
    }

    [SwaggerOperation("A logged-in user no longer follows a source from a manga")]
    [HttpDelete("manga/{mangaId:int}/source/{sourceId:int}")]
    public async Task DeleteUserManga([FromQuery] DeleteFollowedSourceCommand request)
    {
        await mediator.Send(request);
    }
    
    [SwaggerOperation("A logged-in user changes its last chapter read from a combination of manga and source")]
    [HttpPatch("manga/{mangaId:int}/source/{sourceId:int}")]
    public async Task UpdateManga([FromRoute] int mangaId, [FromRoute] int sourceId, [FromBody] UpdateChapterRequest requestBody)
    {
        await mediator.Send(new UpdateUserChapterCommand(mangaId, sourceId, requestBody.ChapterId));
    }
    
    [SwaggerOperation("Changes email for the logged-in user")]
    [HttpPost("account/email")]
    public async Task UpdateUserEmail([FromBody] UpdateUserEmailCommand request)
    {
        await mediator.Send(request);
    }

    [SwaggerOperation("Changes password for the logged-in user")]
    [HttpPost("account/password")]
    public async Task ChangeLoggedUserPassword([FromBody] UpdateUserPasswordCommand request)
    {
        await mediator.Send(request);
    }
}