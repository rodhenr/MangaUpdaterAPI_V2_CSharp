using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.API.Controllers.Shared;
using MangaUpdater.Application.Interfaces.External.MangaDex;
using MangaUpdater.Application.Interfaces.External.MangaLivre;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Application.Models.External.MangaDex;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.API.Controllers;

public class MangaController : BaseController
{
    private string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    private readonly IMangaService _mangaService;
    private readonly IUserSourceService _userSourceService;
    private readonly IRegisterMangaFromMyAnimeListService _registerMangaFromMyAnimeListService;
    private readonly IMangaLivreService _mangaLivreService;
    private readonly IMangaSourceService _mangaSourceService;
    private readonly IChapterService _chapterService;
    private readonly IUserMangaService _userMangaService;
    private readonly IGenreService _genreService;
    private readonly IMangaGenreService _mangaGenreService;
    private readonly IMangaDexApi _mangaDexService;

    public MangaController(IMangaService mangaService, IUserSourceService userSourceService,
        IRegisterMangaFromMyAnimeListService registerMangaFromMyAnimeListService, IMangaLivreService mangaLivreService,
        IMangaSourceService mangaSourceService, IChapterService chapterService, IUserMangaService userMangaService,
        IGenreService genreService, IMangaGenreService mangaGenreService, IMangaDexApi mangaDexService)
    {
        _mangaService = mangaService;
        _userSourceService = userSourceService;
        _registerMangaFromMyAnimeListService = registerMangaFromMyAnimeListService;
        _mangaLivreService = mangaLivreService;
        _mangaSourceService = mangaSourceService;
        _chapterService = chapterService;
        _userMangaService = userMangaService;
        _genreService = genreService;
        _mangaGenreService = mangaGenreService;
        _mangaDexService = mangaDexService;
    }

    [AllowAnonymous]
    [HttpGet("mangadex")]
    public async Task<ActionResult<List<MangaDexResponse>>> GetMangaDex(string id)
    {
        return Ok(await _mangaDexService.GetChaptersAsync(id));
    }

    /// <summary>
    /// Get all manga.
    /// </summary>
    /// <returns>All manga, if any.</returns>
    /// <response code="200">Returns all existing manga, if any.</response>
    [AllowAnonymous]
    [SwaggerOperation("Get all manga")]
    [HttpGet]
    public async Task<ActionResult<MangaResponse>> GetMangas([FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [SwaggerParameter("Empty (no ordering), alphabet or latest")] [FromQuery]
        string? orderBy = null,
        [FromQuery] List<int>? sourceId = null,
        [FromQuery] List<int>? genreId = null,
        [FromQuery] string? input = null)
    {
        var mangaDto = await _mangaService.GetWithFilter(page, pageSize, orderBy, sourceId, genreId, input);
        var genreIdList = await _mangaGenreService.GetUniqueGenresId();
        var genres = await _genreService.GetGenresByListId(genreIdList);

        var mangaData = new MangasWithGenresDto(mangaDto.Mangas, genres);

        return Ok(new MangaResponse(page, pageSize, mangaDto.NumberOfPages, mangaData));
    }

    /// <summary>
    /// Register a new manga using a MyAnimeList id.
    /// </summary>
    /// <returns>Manga data, if success.</returns>
    /// <response code="200">Returns the registered manga data.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Register a new manga using a MyAnimeList id")]
    [HttpPost]
    public async Task<ActionResult<Manga>> RegisterManga(int malId)
    {
        return Ok(await _registerMangaFromMyAnimeListService.RegisterMangaFromMyAnimeListById(malId));
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
    public async Task<ActionResult<MangaDataWithHighlightedMangasDto>> GetManga(int mangaId, [FromQuery] int quantity = 4)
    {
        if (!string.IsNullOrEmpty(Request.Headers["Authorization"]) && UserId is null)
            return Unauthorized("Invalid token data");

        return UserId is not null
            ? Ok(await _mangaService.GetByIdAndUserId(mangaId, UserId!, quantity))
            : Ok(await _mangaService.GetByIdNotLogged(mangaId, quantity));
    }

    /// <summary>
    /// Get all sources from a manga with following info for a logged-in user.
    /// </summary>
    /// <returns>All manga sources, if any.</returns>
    /// <response code="200">Returns all manga sources.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Get all sources from a manga with following info for a logged-in user")]
    [HttpGet("{mangaId:int}/sources")]
    public async Task<ActionResult<IEnumerable<UserSourceDto>>> GetUserSources(int mangaId)
    {
        var userManga = await _userMangaService.GetByUserIdAndMangaId(UserId!, mangaId);
        return Ok(await _userSourceService.GetUserSourcesByMangaId(mangaId, userManga?.Id));
    }

    /// <summary>
    /// Register a new source for a manga.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">Error.</response>
    [SwaggerOperation("Register a new source for a manga.")]
    [HttpPost("/{mangaId:int}/source/{sourceId:int}")]
    public async Task<ActionResult> AddSourceToMangaAndGetData(int mangaId, int sourceId, string mangaUrl)
    {
        if (sourceId == 1)
        {
            await _mangaLivreService.RegisterSourceAndChapters(mangaId, sourceId, mangaUrl);
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
    public async Task<ActionResult> UpdateChaptersFromSource(int mangaId, int sourceId)
    {
        var mangaSource = await _mangaSourceService.GetByMangaIdAndSourceId(mangaId, sourceId);
        var lastChapter = await _chapterService.GetLastByMangaIdAndSourceId(mangaId, sourceId);

        if (sourceId == 1)
        {
            await _mangaLivreService.UpdateChapters(mangaId, sourceId, lastChapter?.Number ?? "0", mangaSource.Url);
        }

        return Ok();
    }
}