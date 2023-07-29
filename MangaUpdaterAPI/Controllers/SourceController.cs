using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SourceController : ControllerBase
{
    private readonly IUserSourceService _userSourceService;

    public SourceController(IUserSourceService userSourceService)
    {
        _userSourceService = userSourceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetMangaSources(int userId, int mangaId)
    {
        var userSources = await _userSourceService.GetAllSourcesByMangaIdWithUserStatus(mangaId, userId);

        return Ok(userSources);
    }
}
