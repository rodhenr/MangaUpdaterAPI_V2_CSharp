using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Application.Models;
using MangaUpdater.Infra.Data.ExternalServices;

namespace MangaUpdater.API.Controllers;

public class SourceController : BaseController
{
    private readonly ISourceService _sourceService;
    private readonly MangaLivreApi _mangaLivreApiService;

    public SourceController(ISourceService sourceService, MangaLivreApi mangaLivreApiService)
    {
        _sourceService = sourceService;
        _mangaLivreApiService = mangaLivreApiService;
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
        return Ok(await _sourceService.Get());
    }

    /// <summary>
    /// Get a source by id.
    /// </summary>
    /// <returns>A source, if exists.</returns>
    /// <response code="200">Returns a source, if exist.</response>
    /// <response code="400">If doesn't exist.</response>
    [SwaggerOperation("Get a source by id")]
    [HttpGet("{sourceId:int}")]
    public async Task<ActionResult<Source>> GetSourceById(int sourceId)
    {
        return Ok(await _sourceService.GetById(sourceId));
    }

    [HttpPost("test")]
    public async Task<ActionResult<List<MangaLivreChapters>>> GetChaptersML(int serieId)
    {
        var mltest = await _mangaLivreApiService.GetChaptersAsync(serieId, 32);

        return mltest;
    }
}