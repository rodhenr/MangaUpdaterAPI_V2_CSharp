using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;

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
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager, IMangaService mangaService,
        IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService,
        IChapterService chapterService, IUserMangaService userMangaService)
    {
        _userManager = userManager;
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
        _chapterService = chapterService;
        _userMangaService = userMangaService;
    }

    [SwaggerOperation("Get all mangas that the user follows with the last 3 released chapters")]
    [HttpGet("mangas")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MangaUserLoggedDto>>> GetLoggedUserMangas()
    {
        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        var mangas =
            await _userMangaChapterService.GetUserMangasWithThreeLastChapterByUserId(user.Id);

        return Ok(mangas);
    }

    [SwaggerOperation("User starts following a manga")]
    [HttpPost("mangas/{mangaId:int}")]
    [Authorize]
    public async Task<ActionResult> FollowManga(int mangaId, IEnumerable<int> sourceIdList)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
            return BadRequest($"Manga not found for id {mangaId}");

        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, user.Id);

        await _userMangaChapterService.AddUserMangaBySourceIdList(mangaId, user.Id, sourceIdList, userSources);

        return Ok();
    }

    [SwaggerOperation("User doesn't follow a manga anymore")]
    [HttpDelete("mangas/{mangaId:int}")]
    [Authorize]
    public async Task<ActionResult> UnfollowManga(int mangaId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
            return BadRequest($"Manga not found for id {mangaId}");

        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        await _userMangaChapterService.DeleteUserMangasByMangaId(mangaId, user.Id);

        return Ok();
    }

    [SwaggerOperation("Get all mangas that the user follows with a simplified response")]
    [HttpGet("mangas/list")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetUserMangasList()
    {
        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        var userMangas = await _userMangaService.GetMangasByUserId(user.Id);

        return Ok(userMangas);
    }

    [SwaggerOperation("Get all followed mangas by a user")]
    [HttpGet("{userId}/mangas")]
    public async Task<ActionResult<IEnumerable<MangaUserDto>>> GetUserManga(string userId)
    {
        var userMangas = await _userMangaService.GetMangasByUserId(userId);

        return Ok(userMangas);
    }

    [SwaggerOperation("User starts following a source from a manga")]
    [HttpPost("mangas/{mangaId:int}/sources/{sourceId:int}")]
    [Authorize]
    public async Task<ActionResult> AddUserManga(int mangaId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
            return BadRequest("Manga not found");

        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        await _userMangaChapterService.AddUserManga(mangaId, user.Id, sourceId);

        return Ok();
    }

    [SwaggerOperation("User doesn't follow a source from a manga anymore")]
    [HttpDelete("mangas/{mangaId:int}/sources/{sourceId:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteUserManga(int mangaId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
            return BadRequest("Manga not found");

        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        await _userMangaChapterService.DeleteUserManga(mangaId, user.Id, sourceId);

        return Ok();
    }

    [SwaggerOperation("User changes the last chapter read from a source from a manga")]
    [HttpPatch("mangas/{mangaId:int}/sources/{sourceId:int}")]
    [Authorize]
    public async Task<ActionResult> UpdateManga(int mangaId, int sourceId, int chapterId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
            return BadRequest("Manga not found");

        var chapter = await _chapterService.GetChapterById(chapterId);

        if (chapter == null || chapter.Source!.Id != sourceId)
            return BadRequest("Invalid chapter/source");

        var emailClaim = User.FindFirst("email")?.Value;

        if (emailClaim == null)
            return BadRequest("Invalid user");

        var user = await _userManager.FindByEmailAsync(emailClaim);

        if (user == null)
            return BadRequest("Invalid user");

        await _userMangaService.UpdateUserMangaAsync(user.Id, mangaId, sourceId, chapterId);

        return Ok();
    }
}