using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Core.Features.Mangas;

namespace MangaUpdater.API.Controllers;

public class MangaController(ISender mediator) : BaseController
{
    /// <summary>
    /// Get all manga.
    /// </summary>
    /// <returns>All manga, if any.</returns>
    /// <response code="200">Returns all existing manga, if any.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get all mangas")]
    [HttpGet]
    public async Task<GetMangasResponse> GetMangas()
    {
        return await mediator.Send(new GetMangasQuery());
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
    /// Register a new manga using a MyAnimeList id.
    /// </summary>
    /// <returns>Manga data, if success.</returns>
    /// <response code="200">Returns the registered manga data.</response>
    /// <response code="400">Error.</response>
    /// <response code="403">Unauthorized</response>
    [Authorize(Policy = "Admin")]
    [SwaggerOperation("Register a new manga using a MyAnimeList id")]
    [HttpPost]
    public async Task<AddMangaResponse> RegisterManga(int malId)
    {
        return await mediator.Send(new AddMangaQuery(malId));
    }

    /// <summary>
    /// Retrieves the total number of users who are currently following a specific manga
    /// </summary>
    /// <response code="200">Returns the total number of users who are currently following a manga.</response>
    /// <response code="400">Error.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get the total number of users who are currently following a manga")]
    [HttpGet("{mangaId:int}/follows")]
    public async Task<GetFollowsResponse> GetUsersFollowingByMangaId([FromQuery] GetFollowsQuery request)
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
    [HttpPost("manga/{mangaId:int}/sources")]
    public async Task<AddMangaSourceResponse> AddSourceToManga([FromQuery] int mangaId, [FromBody] int sourceId, [FromBody] string mangaUrl)
    {
        return await mediator.Send(new AddMangaSourceQuery(mangaId, sourceId, mangaUrl));
    }
    
    [HttpGet("/{mangaId:int}/chapter/{chapterId:int}")]
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
    [HttpPost("/{mangaId:int}/source/{sourceId:int}/chapters")]
    public async Task<UpdateChaptersResponse> UpdateChaptersFromSource([FromQuery] UpdateChaptersQuery request)
    {
        return await mediator.Send(request);
    }
}