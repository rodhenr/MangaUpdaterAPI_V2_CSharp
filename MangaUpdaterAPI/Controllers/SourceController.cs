using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SourceController : ControllerBase
{
    private readonly ISourceService _sourceService;

    public SourceController(ISourceService sourceService)
    {
        _sourceService = sourceService;
    }

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