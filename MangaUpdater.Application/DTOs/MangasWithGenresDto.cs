using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.DTOs;

public record MangasWithGenresDto(IEnumerable<MangaUserDto> Mangas, IEnumerable<Genre> Genres);