using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Domain.Interfaces;

public interface IMangasRegisterRepository
{
    Task CreateAsync(MangaRegister mangaInfo);

    Task<MangaRegister?> GetByIdAsync(int id);
}
