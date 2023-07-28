using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdaterAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaService _userMangaService;

    public HomeController(IMangaService mangaService, IUserMangaService userMangaService)
    {
        _mangaService = mangaService;
        _userMangaService = userMangaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetMangas(int userId)
    {
        var mangas = await _userMangaService.GetMangasByUserId(userId);

        return Ok(mangas);
    }
}
