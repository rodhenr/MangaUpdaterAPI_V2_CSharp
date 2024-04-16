using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Dtos;

public record MangasWithGenresDto(IEnumerable<MangaUserDto> Mangas, IEnumerable<Genre> Genres);