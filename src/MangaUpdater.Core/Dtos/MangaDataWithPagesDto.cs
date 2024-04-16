namespace MangaUpdater.Core.Dtos;

public record MangaDataWithPagesDto(IEnumerable<MangaUserDto> Mangas, int NumberOfPages);