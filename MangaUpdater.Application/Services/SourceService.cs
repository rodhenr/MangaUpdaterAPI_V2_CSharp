﻿using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class SourceService: ISourceService
{
    private readonly ISourceRepository _sourceRepository;
    public SourceService(ISourceRepository sourceRepository)
    {
        _sourceRepository = sourceRepository;
    }

    public async Task<Source?> GetById(int id)
    {
        return await _sourceRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Source>> GetSources()
    {
        return await _sourceRepository.GetSourcesAsync();
    }
}