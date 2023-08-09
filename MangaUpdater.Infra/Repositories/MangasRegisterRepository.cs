using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.Repositories;

public class MangasRegisterRepository : IMangasRegisterRepository
{
    private readonly MangaUpdaterContext _context;

    public MangasRegisterRepository(MangaUpdaterContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(MangaRegister mangaInfo)
    {
        await _context.AddAsync(mangaInfo);
        await _context.SaveChangesAsync();

        return;
    }

    public async Task<MangaRegister?> GetByIdAsync(int id)
    {
        return await _context.MangaRegisters.SingleOrDefaultAsync(a => a.Id == id);
    }
}
