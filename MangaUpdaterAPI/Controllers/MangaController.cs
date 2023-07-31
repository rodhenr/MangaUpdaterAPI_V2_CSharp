using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaChapterService _userMangaChapterService;

    public MangaController(IMangaService mangaService, IUserMangaChapterService userMangaChapterService)
    {
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetMangaById(int mangaId, int userId)
    {
        var manga = await _mangaService.GetMangaByIdAndUserId(mangaId, userId);

        if(manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        return Ok(manga);
    }

    [HttpPost("/follow")]
    public async Task<ActionResult> FollowManga(int mangaId, int userId, IEnumerable<int> sourceIdList)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if(manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.AddUserMangaBySourceIdList(mangaId, userId, sourceIdList);

        return Ok();
    }

    /*[HttpPost]
    public async Task<ActionResult> MarkChapterAsRead()
    {
        return Ok();
    }*/
}
