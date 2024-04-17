using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Core.Features.Sources;

namespace MangaUpdater.API.Controllers;

public class SourceController(ISender mediator) : BaseController
{
    /// <summary>
    /// Get all sources.
    /// </summary>
    /// <returns>All sources, if any.</returns>
    /// <response code="200">Returns all existing sources, if any.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get all sources")]
    [HttpGet]
    public async Task<GetSourcesResponse> GetSources()    
    {
        return await mediator.Send(new GetSourcesQuery());
    }

    /// <summary>
    /// Get a source by id.
    /// </summary>
    /// <returns>A source, if exists.</returns>
    /// <response code="200">Returns a source, if exist.</response>
    /// <response code="400">If doesn't exist.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get a source by id")]
    [HttpGet("{sourceId:int}")]
    public async Task<GetSourceResponse> GetSourceById([FromQuery] GetSourceQuery request)
    {
        return await mediator.Send(request);
    }
}