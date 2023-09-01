using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Application.Services;

public class SourceService : ISourceService
{
    private readonly ISourceRepository _sourceRepository;

    public SourceService(ISourceRepository sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public async Task<IEnumerable<Source>> Get()
    {
        return await _sourceRepository.Get().ToListAsync();
    }

    public async Task<Source?> GetById(int id)
    {
        return await _sourceRepository.GetByIdAsync(id);
    }
}