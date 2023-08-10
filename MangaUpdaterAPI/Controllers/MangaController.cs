using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserSourceService _userSourceService;
    private readonly IChapterService _chapterService;
    private readonly IUserMangaService _userMangaService;
    private readonly IRegisterMangaService _registerMangaService;

    public MangaController(IMangaService mangaService, IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService, IChapterService chapterService, IUserMangaService userMangaService, IRegisterMangaService registerMangaService)
    {
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
        _chapterService = chapterService;
        _userMangaService = userMangaService;
        _registerMangaService = registerMangaService;
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
    public async Task<ActionResult<MangaDTO>> GetManga(int mangaId, int userId)
    {
        var manga = await _mangaService.GetMangaByIdAndUserId(mangaId, userId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        return Ok(manga);
    }

    [SwaggerOperation("Get all sources from a manga")]
    [HttpGet("{mangaId}/sources")]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetUserSources(int mangaId, int userId)
    {
        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);

        if (userSources == null)
        {
            return BadRequest($"No sources found for mangaId {mangaId}");
        }

        return Ok(userSources);
    }    
}
