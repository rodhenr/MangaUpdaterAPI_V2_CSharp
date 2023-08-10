using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaRegisterService
{
    Task<MangaRegister?> CreateMangaRegister(int malMangaId);
}
