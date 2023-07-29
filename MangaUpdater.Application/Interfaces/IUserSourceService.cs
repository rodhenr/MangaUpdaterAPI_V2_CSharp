using MangaUpdater.Application.DTOs;

namespace MangaUpdater.Application.Interfaces;

public interface IUserSourceService
{
    Task<IEnumerable<UserSourceDTO>> GetAllSourcesByMangaIdWithUserStatus(int mangaId, int userId);
}
