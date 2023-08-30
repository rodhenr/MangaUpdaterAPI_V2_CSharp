using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly IMangaSourceService _mangaSourceService;
    private readonly IUserSourceService _userSourceService;
    private readonly IChapterService _chapterService;
    private readonly IRegisterMangaService _registerMangaService;
    private readonly IUpdateChaptersService _updateChaptersService;
    private readonly IRegisterSourceService _registerSourceService;
    private readonly UserManager<IdentityUser> _userManager;

    public MangaController(UserManager<IdentityUser> userManager, IMangaService mangaService,
        IUserSourceService userSourceService, IRegisterMangaService registerMangaService,
        IUpdateChaptersService updateChaptersService, IRegisterSourceService registerSourceService,
        ISourceService sourceService, IMangaSourceService mangaSourceService, IChapterService chapterService)
    {
        _userManager = userManager;
        _mangaService = mangaService;
        _userSourceService = userSourceService;
        _registerMangaService = registerMangaService;
        _updateChaptersService = updateChaptersService;
        _registerSourceService = registerSourceService;
        _sourceService = sourceService;
        _mangaSourceService = mangaSourceService;
        _chapterService = chapterService;
    }

    /// <summary>
    /// Get all manga.
    /// </summary>
    /// <returns>All manga, if any.</returns>
    /// <response code="200">Returns all existing manga, if any.</response>
    [SwaggerOperation("Get all manga")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetMangas([FromQuery] string? orderBy = null,
        [FromQuery] List<int>? sourceId = null, [FromQuery] List<int>? genreId = null)
    {
        return Ok(await _mangaService.GetMangasWithFilter(orderBy, sourceId, genreId));
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
        if (UserId is not null) return (await _mangaService.GetMangaByIdAndUserId(mangaId, UserId!)!);

        return Ok(await _mangaService.GetMangaNotLoggedById(mangaId));
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
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
            return BadRequest("Manga not found");

        if (manga.MangaSources!.Any(ms => ms.SourceId == sourceId))
            return BadRequest($"Source already registered for source id {sourceId}");

        var source = await _sourceService.GetSourcesById(sourceId);

        var chapters =
            _registerSourceService.RegisterFromMangaLivreSource(source!.BaseUrl, mangaUrl, manga.Name);

        if (chapters.Count == 0)
            return BadRequest($"No chapters found");

        var chapterList = chapters.Select(ch =>
            new Chapter(mangaId, sourceId, DateTime.Parse(ch.Value), float.Parse(ch.Key))).ToList();

        await _mangaSourceService.AddMangaSource(new MangaSource(mangaId, sourceId, mangaUrl));
        await _chapterService.BulkCreate(chapterList);

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
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null || manga.MangaSources?.Count == 0 ||
            !manga.MangaSources.Any(ms => ms.SourceId == sourceId))
            return BadRequest("Manga not found");

        var source = await _sourceService.GetSourcesById(sourceId);

        if (source == null)
            return BadRequest("Source not found");

        var chapters = _updateChaptersService.UpdateChaptersFromMangaLivreSource(source.BaseUrl,
            manga.MangaSources.First(ms => ms.SourceId == sourceId).Url);

        await _chapterService.CreateOrUpdateChaptersByMangaSource(mangaId, sourceId, chapters);

        return Ok();
    }
}