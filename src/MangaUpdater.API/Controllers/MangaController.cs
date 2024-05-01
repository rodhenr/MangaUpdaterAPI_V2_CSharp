﻿using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Core.Features.External;
using MangaUpdater.Core.Features.Genres;
using MangaUpdater.Core.Features.Mangas;
using MangaUpdater.Core.Features.MangaSources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

public class MangaController(IMediator mediator) : BaseController
{
    [AllowAnonymous]
    [SwaggerOperation("Get all mangas")]
    [HttpGet]
    public async Task<GetMangasResponse> GetMangas([FromQuery] GetMangasQuery request)
    {
        return await mediator.Send(request);
    }
    
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Register a new manga using a MyAnimeList id")]
    [HttpPost]
    public async Task RegisterManga([FromBody] AddMangaCommand request)
    {
        await mediator.Send(request);
    }
    
    [AllowAnonymous]
    [SwaggerOperation("Get manga data by id")]
    [HttpGet("{mangaId:int}")]
    public async Task<GetMangaResponse> GetManga([FromQuery] GetMangaQuery request)
    {
        return await mediator.Send(request);
    }
    
    [AllowAnonymous]
    [SwaggerOperation("Get all genres")]
    [HttpGet("genre")]
    public async Task<List<GetGenresResponse>> GetGenres()
    {
        return await mediator.Send(new GetGenresQuery());
    }
    
    [AllowAnonymous]
    [SwaggerOperation("Get the total number of users who are currently following a manga")]
    [HttpGet("{mangaId:int}/follows")]
    public async Task<GetMangaFollowersCountResponse> GetUsersFollowingByMangaId([FromQuery] GetMangaFollowersCountQuery request)
    {
        return await mediator.Send(request);
    }
    
    [SwaggerOperation("Get a chapter for a manga")]
    [HttpGet("{mangaId:int}/chapter/{chapterId:int}")]
    public async Task<GetChapterResponse> GetChaptersByIdAndMangaId([FromQuery] GetChapterQuery request)
    {
        return await mediator.Send(request);
    }
    
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Register a new source for a manga.")]
    [HttpPost("{mangaId:int}/source")]
    public async Task AddSourceToManga(int mangaId, [FromBody] SourceInfo sourceInfo)
    {
        await mediator.Send(new AddMangaSourceCommand(mangaId, sourceInfo));
    }
    
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Update chapters from a combination of manga and source.")]
    [HttpPost("{mangaId:int}/source/{sourceId:int}/chapter")]
    public async Task UpdateChaptersFromSource([FromQuery] UpdateChaptersCommand request)
    {
        await mediator.Send(request);
    }

    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Update all manga url from AsuraScans")]
    [HttpPost("update/asura")]
    public async Task UpdateMangaUrlFromAsuraScans()
    {
        await mediator.Send(new UpdateMangaUrlFromAsuraScansCommand());
    }
}