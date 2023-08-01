using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SourceController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserSourceService _userSourceService;

    public SourceController(IMangaService mangaService, IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService)
    {
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetMangaSources(int userId, int mangaId)
    {
        var userSources = await _userSourceService.GetAllSourcesByMangaIdWithUserStatus(mangaId, userId);

        return Ok(userSources);
    }

    [HttpPost("source/follow")]
    public async Task<ActionResult> FollowSource(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.AddUserSource(mangaId, userId, sourceId);

        return Ok();
    }

    [HttpPost("source/unfollow")]
    public async Task<ActionResult> UnfollowSource(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.DeleteUserSource(mangaId, userId, sourceId);

        return Ok();
    }
}
