using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Helpers;
using MangaUpdater.Application.Interfaces;
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

    public async Task<MangaDataWithPagesDto> GetWithFilter(int page, int pageSize, string? orderBy, List<int>? sourceIdList, List<int>? genreIdList, string? input)
    { 
        var query = _mangaRepository.GetWithFiltersAsync(orderBy, sourceIdList, genreIdList, input);
        
        var mangas = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        
        var numberOfMangas = await query
            .AsNoTracking()
            .Select(m => m.Id)
            .ToListAsync();
        
        var mangaUserDtoList = mangas
            .Select(manga =>
                new MangaUserDto(manga.Id, manga.CoverUrl, manga.MangaTitles!.First().Name))
            .ToList();
        
        var numberOfPages = (int)Math.Ceiling((double)numberOfMangas.Count / pageSize);

        return new MangaDataWithPagesDto(mangaUserDtoList, numberOfPages);
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