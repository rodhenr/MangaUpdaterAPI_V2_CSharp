using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.API.Controllers.Shared;

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
    [SwaggerOperation("Get all sources")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Source>>> GetSources()
    {
        var sources = await _sourceService.GetSources();

        return Ok(sources);
    }

    [SwaggerOperation("Get a source")]
    [HttpGet("{sourceId:int}")]
    public async Task<ActionResult<Source>> GetSourceById(int sourceId)
    {
        var source = await _sourceService.GetSourcesById(sourceId);

        if (source == null)
            return BadRequest($"Source not found by id {sourceId}");

        return Ok(source);
    }
}