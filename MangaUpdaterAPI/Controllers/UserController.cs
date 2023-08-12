using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MangaUpdater.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserSourceService _userSourceService;
    private readonly IChapterService _chapterService;
    private readonly IUserMangaService _userMangaService;

    public UserController(IMangaService mangaService, IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService, IChapterService chapterService, IUserMangaService userMangaService)
    {
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
        _chapterService = chapterService;
        _userMangaService = userMangaService;
    }

    [SwaggerOperation("Get all mangas that the user follows with the last 3 released chapters")]
    [HttpGet("mangas")]
    public async Task<ActionResult<IEnumerable<MangaUserLoggedDTO>>> GetLoggedUserMangas(int userId)
    {
        IEnumerable<MangaUserLoggedDTO> mangas = await _userMangaChapterService.GetUserMangasWithThreeLastChapterByUserId(userId);

        return Ok(mangas);
    }

    [SwaggerOperation("User starts following a manga")]
    [HttpPost("mangas/{mangaId}")]
    public async Task<ActionResult> FollowManga(int mangaId, int userId, IEnumerable<int> sourceIdList)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);

        await _userMangaChapterService.AddUserMangaBySourceIdList(mangaId, userId, sourceIdList, userSources);

        return Ok();
    }

    [SwaggerOperation("User doesn't follow a manga anymore")]
    [HttpDelete("mangas/{mangaId}")]
    public async Task<ActionResult> UnfollowManga(int mangaId, int userId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        await _userMangaChapterService.DeleteUserMangasByMangaId(mangaId, userId);

        return Ok();
    }

    [SwaggerOperation("Get all mangas that the user follows with a simplified response")]
    [HttpGet("mangas/list")]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetUserMangasList(int userId)
    {
        //Needs to implement JWT token to check the userId
        IEnumerable<MangaUserDTO> userMangas = await _mangaService.GetMangasByUserId(userId);

        return Ok(userMangas);
    }

    [SwaggerOperation("Get all followed mangas by a user")]
    [HttpGet("{userId}/mangas")]
    public async Task<ActionResult<IEnumerable<MangaUserDTO>>> GetUserMangas(int userId)
    {
        IEnumerable<MangaUserDTO> userMangas = await _mangaService.GetMangasByUserId(userId);

        return Ok(userMangas);
    }

    [SwaggerOperation("User starts following a source from a manga")]
    [HttpPost("mangas/{mangaId}/sources/{sourceId}")]
    public async Task<ActionResult> AddUserManga(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.AddUserManga(mangaId, userId, sourceId);

        return Ok();
    }

    [SwaggerOperation("User doesn't follow a source from a manga anymore")]
    [HttpDelete("mangas/{mangaId}/sources/{sourceId}")]
    public async Task<ActionResult> DeleteUserManga(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.DeleteUserManga(mangaId, userId, sourceId);

        return Ok();
    }

    [SwaggerOperation("User changes the last chapter read from a source from a manga")]
    [HttpPatch("mangas/{mangaId}/sources/{sourceId}")]
    public async Task<ActionResult> UpdateManga(int mangaId, int sourceId, int userId, int chapterId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        var chapter = await _chapterService.GetChapterById(chapterId);

        if (chapter == null)
        {
            return BadRequest("Chapter not found");
        }

        await _userMangaService.UpdateUserMangaAsync(userId, mangaId, sourceId, chapterId);

        return Ok();
    }
}
