﻿using MangaUpdater.Application.DTOs;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Interfaces;

public interface IMangaService
{
    Task AddManga(Manga manga);
    Task<IEnumerable<Manga>> GetMangas();
    Task<IEnumerable<MangaUserDTO>> GetMangasWithFilter(string? orderBy, List<int>? sourceIdList, List<int>? genreIdList); 
    Task<Manga?> GetMangaById(int id);
    Task<Manga?> GetMangaByMalId(int malId);
    Task<MangaDTO?> GetMangaByIdAndUserId(int id, string userId);
}
