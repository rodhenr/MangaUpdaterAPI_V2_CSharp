using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Scraping;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly ISourceService _sourceService;
    private readonly IMangaSourceService _mangaSourceService;
    private readonly IUserSourceService _userSourceService;
    private readonly IChapterService _chapterService;
    private readonly IRegisterMangaService _registerMangaService;
    private readonly IUpdateChaptersService _updateChaptersService;
    private readonly IRegisterSourceService _registerSourceService;
    private readonly IUserService _userService;

    public MangaController(IMangaService mangaService, IUserSourceService userSourceService, IRegisterMangaService registerMangaService, IUpdateChaptersService updateChaptersService, IRegisterSourceService registerSourceService, ISourceService sourceService, IMangaSourceService mangaSourceService, IChapterService chapterService, IUserService userService)
    {
        _mangaService = mangaService;
        _userSourceService = userSourceService;
        _registerMangaService = registerMangaService;
        _updateChaptersService = updateChaptersService;
        _registerSourceService = registerSourceService;
        _sourceService = sourceService;
        _mangaSourceService = mangaSourceService;
        _chapterService = chapterService;
        _userService = userService;
    }

    [SwaggerOperation("Get all mangas")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetMangas([FromQuery] string? orderBy = null, [FromQuery] List<int>? sourceId = null, [FromQuery] List<int>? genreId = null)
    {
        var mangas = await _mangaService.GetMangasWithFilter(orderBy, sourceId, genreId);

        return Ok(mangas);
    }

    [SwaggerOperation("Register a new manga")]
    [HttpPost]
    public async Task<ActionResult<Manga>> RegisterManga(int malId)
    {
        Manga? mangaSearch = await _mangaService.GetMangaByMalId(malId);

        if (mangaSearch != null)
        {
            return Ok(mangaSearch);
        }

        Manga? mangaData = await _registerMangaService.RegisterMangaFromMyAnimeListById(malId);

        if (mangaData == null)
        {
            return BadRequest($"Manga not found for id {malId}");
        }

        return Ok(mangaData);
    }

    [SwaggerOperation("Get a manga")]
    [HttpGet("{mangaId}")]
    public async Task<ActionResult<MangaDTO>> GetManga(int mangaId, int userId = 0)
    {
        //NEEDS TO SEPARATE IN 2 METHODS
        var manga = await _mangaService.GetMangaByIdAndUserId(mangaId, userId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        return Ok(manga);
    }

    [SwaggerOperation("Get all sources from a manga")]
    [HttpGet("{mangaId}/sources")]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetUserSources(int mangaId, int userId = 0)
    {
        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);

        if (userSources == null)
        {
            return BadRequest($"No sources found for mangaId {mangaId}");
        }

        return Ok(userSources);
    }

    [SwaggerOperation("Register and update source from a registered manga")]
    [HttpPost("/{mangaId}/source/{sourceId}/scraping")]
    public async Task<ActionResult> RegisterFromScraping(int mangaId, int sourceId, string mangaUrl)
    {
        var mangaSources = await _mangaSourceService.GetAllByMangaId(mangaId);

        if (mangaSources == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }
        else if (mangaSources.Any(ms => ms.SourceId == sourceId))
        {
            return BadRequest($"Source already registered for source id ${sourceId}");
        }

        var source = await _sourceService.GetSourcesById(sourceId);

        if (source == null)
        {
            return BadRequest($"Source not found for id {sourceId}");
        }

        var chapters = _registerSourceService.RegisterFromMangaLivreSource(source.BaseURL, mangaUrl, mangaSources.Select(ms => ms.Manga.Name).First());

        if (chapters.Count == 0)
        {
            return BadRequest($"No chapters found");
        }

        try
        {
            await _mangaSourceService.AddMangaSource(new MangaSource(mangaId, sourceId, mangaUrl));

            await _chapterService.BulkCreate(mangaId, sourceId, chapters);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
