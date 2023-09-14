using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Scraping;
using MangaUpdater.Domain.Entities;
using MangaUpdater.API.Controllers.Shared;

namespace MangaUpdater.API.Controllers;

public class MangaController : BaseController
{
    private string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    private readonly IMangaService _mangaService;
    private readonly ISourceService _sourceService;
    private readonly IMangaTitleService _mangaTitleService;
    private readonly IUserSourceService _userSourceService;
    private readonly IChapterService _chapterService;
    private readonly IRegisterMangaService _registerMangaService;
    private readonly IUpdateChaptersService _updateChaptersService;
    private readonly IRegisterSourceService _registerSourceService;

    public MangaController(IMangaService mangaService,
        IUserSourceService userSourceService, IRegisterMangaService registerMangaService,
        IUpdateChaptersService updateChaptersService, IRegisterSourceService registerSourceService,
        ISourceService sourceService, IMangaTitleService mangaTitleService, IChapterService chapterService)
    {
        _mangaService = mangaService;
        _userSourceService = userSourceService;
        _registerMangaService = registerMangaService;
        _updateChaptersService = updateChaptersService;
        _registerSourceService = registerSourceService;
        _sourceService = sourceService;
        _mangaTitleService = mangaTitleService;
        _chapterService = chapterService;
    }

    /// <summary>
    /// Get all manga.
    /// </summary>
    /// <returns>All manga, if any.</returns>
    /// <response code="200">Returns all existing manga, if any.</response>
    [SwaggerOperation("Get all manga")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetMangas([FromQuery] int page = 1,
        [SwaggerParameter("Empty (no ordering), alphabet or latest")][FromQuery] string? orderBy = null,
        [FromQuery] List<int>? sourceId = null, [FromQuery] List<int>? genreId = null)
    {
        return Ok(await _mangaService.GetWithFilter(page, orderBy, sourceId, genreId));
    }

    /// <summary>
    /// Register a new manga using a MyAnimeList id.
    /// </summary>
    /// <returns>Manga data, if success.</returns>
    /// <response code="200">Returns the registered manga data.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Register a new manga using a MyAnimeList id")]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Manga>> RegisterManga(int malId)
    {
        return Ok(await _registerMangaService.RegisterMangaFromMyAnimeListById(malId));
    }

    /// <summary>
    /// Get manga data by id.
    /// </summary>
    /// <returns>Manga data, if success.</returns>
    /// <response code="200">Returns the manga data.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get manga data by id")]
    [HttpGet("{mangaId:int}")]
    public async Task<ActionResult<MangaDto>> GetManga(int mangaId)
    {
        return UserId is not null
            ? Ok(await _mangaService.GetByIdAndUserId(mangaId, UserId!))
            : Ok(await _mangaService.GetByIdNotLogged(mangaId));
    }

    /// <summary>
    /// Get all sources from a manga with following info for a logged-in user.
    /// </summary>
    /// <returns>All manga sources, if any.</returns>
    /// <response code="200">Returns all manga sources.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all sources from a manga with following info for a logged-in user")]
    [HttpGet("{mangaId:int}/sources")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserSourceDto>>> GetUserSources(int mangaId)
    {
        return Ok(await _userSourceService.GetUserSourcesByMangaId(mangaId, UserId!));
    }

    /// <summary>
    /// Register a new manga from scraping a source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Register a new manga from scraping a source.")]
    [HttpPost("/{mangaId:int}/source/{sourceId:int}/scraping")]
    [Authorize]
    public async Task<ActionResult> RegisterFromScraping(int mangaId, int sourceId, string mangaUrl)
    {
        var manga = await _mangaService.GetById(mangaId);
        var source = await _sourceService.GetById(sourceId);
        var mangaTitles = await _mangaTitleService.GetAllByMangaId(mangaId);

        if (source.Name == "Manga Livre")
        {
            await _registerSourceService.RegisterFromMangaLivreSource(mangaId, sourceId, source!.BaseUrl, mangaUrl,
                mangaTitles);
        }
        else
        {
            await _registerSourceService.RegisterFromAsuraScansSource(mangaId, sourceId, source!.BaseUrl, mangaUrl,
                mangaTitles);
        }

        return Ok();
    }

    /// <summary>
    /// Update chapters from a combination of manga and source.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Update chapters from a combination of manga and source.")]
    [HttpPost("/{mangaId:int}/source/{sourceId:int}/chapters")]
    [Authorize]
    public async Task<ActionResult> UpdateChapterForMangaSource(int mangaId, int sourceId)
    {
        var manga = await _mangaService.GetById(mangaId);
        var source = await _sourceService.GetById(sourceId);

        Dictionary<string, string> chapters;

        if (source.Name == "Manga Livre")
        {
            chapters = _updateChaptersService.UpdateChaptersFromMangaLivreSource(source.BaseUrl,
                manga.MangaSources.First(ms => ms.SourceId == sourceId).Url);
        }
        else
        {
            chapters = _updateChaptersService.UpdateChaptersFromAsuraScansSource(source.BaseUrl,
                manga.MangaSources.First(ms => ms.SourceId == sourceId).Url);
        }

        await _chapterService.CreateOrUpdateByMangaSource(mangaId, sourceId, chapters);

        return Ok();
    }
}