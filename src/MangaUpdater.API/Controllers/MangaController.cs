using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Core.Features.External;
using MangaUpdater.Core.Features.Mangas;
using MangaUpdater.Core.Features.MangaSources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

public class MangaController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Get all manga.
    /// </summary>
    /// <returns>All manga, if any.</returns>
    /// <response code="200">Returns all existing manga, if any.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get all mangas")]
    [HttpGet]
    public async Task<GetMangasResponse> GetMangas([FromQuery] GetMangasQuery request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Register a new manga using a MyAnimeList id.
    /// </summary>
    /// <returns>Manga data, if success.</returns>
    /// <response code="200">Returns the registered manga data.</response>
    /// <response code="400">Error.</response>
    /// <response code="403">Unauthorized</response>
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Register a new manga using a MyAnimeList id")]
    [HttpPost]
    public async Task RegisterManga([FromBody] AddMangaCommand request)
    {
        await mediator.Send(request);
    }

    /// <summary>
    /// Get manga data by id.
    /// </summary>
    /// <returns>Manga data, if success.</returns>
    /// <response code="200">Returns the manga data.</response>
    /// <response code="400">Error.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get manga data by id")]
    [HttpGet("{mangaId:int}")]
    public async Task<GetMangaResponse> GetManga([FromQuery] GetMangaQuery request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Retrieves the total number of users who are currently following a specific manga
    /// </summary>
    /// <response code="200">Returns the total number of users who are currently following a manga.</response>
    /// <response code="400">Error.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get the total number of users who are currently following a manga")]
    [HttpGet("{mangaId:int}/follows")]
    public async Task<GetMangaFollowersCountResponse> GetUsersFollowingByMangaId([FromQuery] GetMangaFollowersCountQuery request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Register a new source for a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    /// <response code="403">Unauthorized</response>
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Register a new source for a manga.")]
    [HttpPost("{mangaId:int}/sources")]
    public async Task AddSourceToManga(int mangaId, [FromBody] SourceInfo sourceInfo)
    {
        await mediator.Send(new AddMangaSourceCommand(mangaId, sourceInfo));
    }
    
    /// <summary>
    /// Get chapter data by manga and source.
    /// </summary>
    /// <returns>Chapter data, if success.</returns>
    /// <response code="200">Returns the manga data.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get a chapter for a manga")]
    [HttpGet("{mangaId:int}/chapter/{chapterId:int}")]
    public async Task<GetChapterResponse> GetChaptersByIdAndMangaId([FromQuery] GetChapterQuery request)
    {
        return await mediator.Send(request);
    }
    
    /// <summary>
    /// Update chapters from a combination of manga and source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    /// <response code="403">Unauthorized</response>
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Update chapters from a combination of manga and source.")]
    [HttpPost("{mangaId:int}/source/{sourceId:int}/chapters")]
    public async Task UpdateChaptersFromSource([FromQuery] UpdateChaptersCommand request)
    {
        await mediator.Send(request);
    }

    [AllowAnonymous]
    [SwaggerOperation("Update all manga url from AsuraScans")]
    [HttpPost("/update/asurascans")]
    public async Task UpdateMangaUrlFromAsuraScans()
    {
        await mediator.Send(new UpdateMangaUrlFromAsuraScansCommand());
    }
}