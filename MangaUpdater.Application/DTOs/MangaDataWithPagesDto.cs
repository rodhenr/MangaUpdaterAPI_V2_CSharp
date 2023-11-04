namespace MangaUpdater.Application.DTOs;

public record MangaDataWithPagesDto(IEnumerable<MangaUserDto> Mangas, int NumberOfPages);