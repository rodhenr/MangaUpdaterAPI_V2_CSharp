using System.Globalization;
using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Models.External;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> CheckIfMangaIsRegistered(int myAnimeListId)
    {
        var manga = await _mangaRepository.GetByMalIdAsync(myAnimeListId);

        return manga is not null;
    }

    public async Task<MangaDataWithPagesDto> GetWithFilter(int page, int pageSize, string? orderBy,
        List<int>? sourceIdList, List<int>? genreIdList, string? input)
    {
        var query = _mangaRepository.GetWithFiltersQueryable(orderBy, sourceIdList, genreIdList, input);

        var mangaUserDtoList = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(manga =>
                new MangaUserDto(manga.Id, manga.CoverUrl, manga.MangaTitles!.First().Name))
            .ToListAsync();

        var numberOfMangas = await query
            .Select(m => m.Id)
            .ToListAsync();

        var numberOfPages = (int)Math.Ceiling((double)numberOfMangas.Count / pageSize);

        return new MangaDataWithPagesDto(mangaUserDtoList, numberOfPages);
    }

    public async Task<MangaDataWithHighlightedMangasDto> GetByIdNotLogged(int id, int quantity)
    {
        var manga = await _mangaRepository.GetByIdOrderedDescAsync(id);
        ValidationHelper.ValidateEntity(manga);

        var mangaDto = _mapper.Map<MangaDto>(manga);

        var highlightedMangas = await _mangaRepository.GetHighlightedAsync(id, quantity);
        var highlightedMangasDto = _mapper.Map<IEnumerable<MangaUserDto>>(highlightedMangas);

        return new MangaDataWithHighlightedMangasDto(mangaDto, highlightedMangasDto);
    }

    public async Task<MangaDataWithHighlightedMangasDto> GetByIdAndUserId(int id, string userId, int quantity)
    {
        var manga = await _mangaRepository.GetByIdAndUserIdOrderedDescAsync(id, userId);
        ValidationHelper.ValidateEntity(manga);

        var mangaDto = _mapper.Map<MangaDto>(manga);

        var highlightedMangas = await _mangaRepository.GetHighlightedAsync(id, quantity);
        var highlightedMangasDto = _mapper.Map<IEnumerable<MangaUserDto>>(highlightedMangas);

        return new MangaDataWithHighlightedMangasDto(mangaDto, highlightedMangasDto);
    }

    public async Task<List<MangaInfoToUpdateChapters>> GetMangasToUpdateChapters()
    {
        var mangaList = await _mangaRepository.GetMangasToUpdateChaptersAsync();

        var mangasToUpdateChapters = mangaList
            .SelectMany(m => m.MangaSources!, (m, ms) =>
            {
                var lastChapter = m.Chapters!
                    .Where(ch => ch.SourceId == ms.SourceId)
                    .OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture))
                    .FirstOrDefault();

                return new MangaInfoToUpdateChapters(m.Id, ms.SourceId, ms.Url, ms.Source!.BaseUrl, ms.Source.Name,
                    lastChapter is null ? "0" : lastChapter.Number);
            })
            .ToList();

        return mangasToUpdateChapters;
    }
}