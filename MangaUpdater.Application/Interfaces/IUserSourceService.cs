using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserSourceService
{
    Task<IEnumerable<UserSourceDto>> GetUserSourcesByMangaId(int mangaId, int? userMangaId);
}