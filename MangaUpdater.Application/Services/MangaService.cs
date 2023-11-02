﻿using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;

public class MangaService : IMangaService
{
    private readonly IMangaRepository _mangaRepository;
    private readonly IMapper _mapper;

    public MangaService(IMangaRepository mangaRepository, IMapper mapper)
    {
        _mangaRepository = mangaRepository;
        _mapper = mapper;
    }

    public async Task Add(Manga manga)
    {
        _mangaRepository.Create(manga);
        await _mangaRepository.SaveAsync();
    }

    public async Task<int> CheckNumberOfPages(int pageSize)
    {
        return await _mangaRepository.CheckNumberOfPagesAsync(pageSize);
    }

    public async Task<bool> CheckIfMangaIsRegistered(int myAnimeListId)
    {
        var manga = await _mangaRepository.GetByMalIdAsync(myAnimeListId);

        return manga is not null;
    }

    public async Task<IEnumerable<MangaUserDto>> GetWithFilter(int page, int pageSize, string? orderBy, List<int>? sourceIdList,
        List<int>? genreIdList)
    {
        var mangas = await _mangaRepository.GetWithFiltersAsync(page, pageSize, orderBy, sourceIdList, genreIdList);

        var mangaUserDtoList = mangas
            .Select(manga =>
                new MangaUserDto(manga.Id, manga.CoverUrl, manga.MangaTitles!.First().Name))
            .ToList();

        return mangaUserDtoList;
    }

    public async Task<MangaDto> GetByIdNotLogged(int id)
    {
        var manga = await _mangaRepository.GetByIdOrderedDescAsync(id);
        ValidationHelper.ValidateEntity(manga);

        return _mapper.Map<MangaDto>(manga);
    }

    public async Task<MangaDto> GetByIdAndUserId(int id, string userId)
    {
        var manga = await _mangaRepository.GetByIdAndUserIdOrderedDescAsync(id, userId);
        ValidationHelper.ValidateEntity(manga);

        return _mapper.Map<MangaDto>(manga);
    }
}