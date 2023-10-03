using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.API.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;

namespace MangaUpdater.API.Controllers;

public class SourceController : BaseController
{
    private readonly ISourceService _sourceService;

    public SourceController(ISourceService sourceService)
    {
        _sourceService = sourceService;
    }

    /// <summary>
    /// Get all sources.
    /// </summary>
    /// <returns>All sources, if any.</returns>
    /// <response code="200">Returns all existing sources, if any.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get all sources")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Source>>> GetSources()
    {
        return Ok(await _sourceService.Get());
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
    public async Task<ActionResult<Source>> GetSourceById(int sourceId)
    {
        return Ok(await _sourceService.GetById(sourceId));
    }
}