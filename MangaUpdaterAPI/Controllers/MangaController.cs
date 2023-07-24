using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdaterAPI.Controllers;

[Route("manga")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly IMangaService _mangaService;

    public MangaController(IMangaService mangaService)
    {
        _mangaService = mangaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetMangas()
    {
        IEnumerable<Manga> mangas = await _mangaService.GetMangas();

        return Ok(mangas);
    }

    [HttpGet("/manga/{id}")]
    public async Task<IActionResult> GetMangaById(int id)
    {
        Manga manga = await _mangaService.GetMangaById(id);

        if(manga is null)
        {
            return NotFound($"No result found for id {id}");
        }

        return Ok(manga);
    }
}
