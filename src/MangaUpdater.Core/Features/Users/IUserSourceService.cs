using MangaUpdater.Core.Dtos;

namespace MangaUpdater.Application.Interfaces;

public interface IUserSourceService
{
    Task<IEnumerable<UserSourceDto>> GetUserSourcesByMangaId(int mangaId, int? userMangaId);
}