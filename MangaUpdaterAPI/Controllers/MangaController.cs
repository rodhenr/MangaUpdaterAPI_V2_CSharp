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

    public MangaController(IMangaService mangaService)
    {
        _mangaService = mangaService;
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

    /*[HttpPost]
    public async Task<ActionResult> FollowManga()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> MarkChapterAsRead()
    {
        return Ok();
    }*/
}
