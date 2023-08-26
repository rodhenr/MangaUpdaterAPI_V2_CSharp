using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Scraping;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using MangaUpdater.Application.Services;
using Microsoft.AspNetCore.Identity;

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
    private readonly UserManager<IdentityUser> _userManager;

    public MangaController(UserManager<IdentityUser> userManager, IMangaService mangaService, IUserSourceService userSourceService, IRegisterMangaService registerMangaService, IUpdateChaptersService updateChaptersService, IRegisterSourceService registerSourceService, ISourceService sourceService, IMangaSourceService mangaSourceService, IChapterService chapterService)
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

    [SwaggerOperation("Get all mangas")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetMangas([FromQuery] string? orderBy = null, [FromQuery] List<int>? sourceId = null, [FromQuery] List<int>? genreId = null)
    {
        var mangas = await _mangaService.GetMangasWithFilter(orderBy, sourceId, genreId);

        return Ok(mangas);
    }

    [SwaggerOperation("Register a new manga")]
    [HttpPost]
    [Authorize]
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
    public async Task<ActionResult<MangaDTO>> GetManga(int mangaId)
    {
        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
        {
            return BadRequest();
        }

        var user = await _userManager.FindByEmailAsync(emailClaim);

        //NEEDS TO SEPARATE IN 2 METHODS
        var manga = await _mangaService.GetMangaByIdAndUserId(mangaId, user.Id); //TODO: Change

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        return Ok(manga);
    }

    [SwaggerOperation("Get all sources from a manga")]
    [HttpGet("{mangaId}/sources")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetUserSources(int mangaId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var emailClaim = User.FindFirst("email")?.Value;

            if (emailClaim == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(emailClaim);

            if (user == null)
            {
                return BadRequest();
            }

            var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, user.Id);

            if (userSources == null)
            {
                return BadRequest($"No sources found for mangaId {mangaId}");
            }

            return Ok(userSources);
        }

        return BadRequest();
    }

    [SwaggerOperation("Register and update source from a registered manga")]
    [HttpPost("/{mangaId}/source/{sourceId}/scraping")]
    [Authorize]
    public async Task<ActionResult> RegisterFromScraping(int mangaId, int sourceId, string mangaUrl)
    {
        try
        {
            var manga = await _mangaService.GetMangaById(mangaId);

            if (manga == null)
            {
                return BadRequest("Manga not found");
            }

            if (manga.MangaSources!.Any(ms => ms.SourceId == sourceId))
            {
                return BadRequest($"Source already registered for source id {sourceId}");
            }

            var source = await _sourceService.GetSourcesById(sourceId);

            var chapters = _registerSourceService.RegisterFromMangaLivreSource(source!.BaseURL, mangaUrl, manga.Name);

            if (chapters.Count == 0)
            {
                return BadRequest($"No chapters found");
            }

            //TODO: Implement transaction
            await _mangaSourceService.AddMangaSource(new MangaSource(mangaId, sourceId, mangaUrl));

            List<Chapter> chapterList = new();

            foreach (var chapter in chapters)
            {
                chapterList.Add(new Chapter(mangaId, sourceId, DateTime.Parse(chapter.Value), float.Parse(chapter.Key)));
            }

            await _chapterService.BulkCreate(chapterList);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [SwaggerOperation("Update chapters for a manga/source")]
    [HttpPost("/{mangaId}/source/{sourceId}/chapters")]
    [Authorize]
    public async Task<ActionResult> UpdateChapterForMangaSource(int mangaId, int sourceId)
    {
        try
        {
            var manga = await _mangaService.GetMangaById(mangaId);

            if (manga == null)
            {
                return BadRequest("Manga not found");
            }

            if (!manga.MangaSources!.Any(ms => ms.SourceId == sourceId))
            {
                return BadRequest($"Source not found");
            }

            var source = await _sourceService.GetSourcesById(sourceId);

            var chapters = _updateChaptersService.UpdateChaptersFromMangaLivreSource(source!.BaseURL, manga.MangaSources!.Where(ms => ms.SourceId == sourceId).First().URL);

            await _chapterService.CreateOrUpdateChaptersByMangaSource(mangaId, sourceId, chapters);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
