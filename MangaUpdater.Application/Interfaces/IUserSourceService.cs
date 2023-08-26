using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserSourceService
{
    Task<IEnumerable<UserSourceDTO>?> GetUserSourcesByMangaId(int mangaId, string userId);
}
