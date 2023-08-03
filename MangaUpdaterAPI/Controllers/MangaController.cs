using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MangaUpdater.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MangaController : ControllerBase
{
    private readonly IMangaService _mangaService;
    private readonly IUserMangaChapterService _userMangaChapterService;
    private readonly IUserSourceService _userSourceService;

    public MangaController(IMangaService mangaService, IUserMangaChapterService userMangaChapterService, IUserSourceService userSourceService)
    {
        _mangaService = mangaService;
        _userMangaChapterService = userMangaChapterService;
        _userSourceService = userSourceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manga>>> GetMangas()
    {
        var mangas = await _mangaService.GetMangas();

        return Ok(mangas);
    }

    [HttpGet("{mangaId}")]
    public async Task<ActionResult<MangaDTO>> GetMangaById(int mangaId, int userId)
    {
        var manga = await _mangaService.GetMangaByIdAndUserId(mangaId, userId);

        if (manga == null)
        {
            return BadRequest($"Manga not found for id {mangaId}");
        }

        return Ok(manga);
    }

    [HttpPost("{mangaId}/follow")]
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

    [HttpDelete("{mangaId}/unfollow")]
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

    [HttpGet("{mangaId}/sources")]
    public async Task<ActionResult<IEnumerable<UserSourceDTO>>> GetSourcesByMandaId(int mangaId, int userId)
    {
        var userSources = await _userSourceService.GetUserSourcesByMangaId(mangaId, userId);

        if (userSources == null)
        {
            return BadRequest($"No sources found for mangaId {mangaId}");
        }

        return Ok(userSources);
    }

    [HttpPost("{mangaId}/sources/follow")]
    public async Task<ActionResult> AddMangaSourceByUser(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.AddUserManga(mangaId, userId, sourceId);

        return Ok();
    }

    [HttpDelete("{mangaId}/sources/unfollow")]
    public async Task<ActionResult> DeleteMangaSourceByUser(int mangaId, int userId, int sourceId)
    {
        var manga = await _mangaService.GetMangaById(mangaId);

        if (manga == null)
        {
            return BadRequest("Manga not found");
        }

        await _userMangaChapterService.DeleteUserManga(mangaId, userId, sourceId);

        return Ok();
    }
}
