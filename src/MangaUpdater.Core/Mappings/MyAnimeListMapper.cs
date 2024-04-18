using MangaUpdater.Core.Models;
using Riok.Mapperly.Abstractions;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Mappings;

[Mapper]
public static partial class MyAnimeListMapper
{
    [MapProperty(nameof(Manga.CoverUrl), nameof(MyAnimeListApiResponse.Images.JPG.LargeImageUrl))]
    [MapProperty(nameof(Manga.MyAnimeListId), nameof(MyAnimeListApiResponse.MalId))]
    public static partial Manga ToModel(MyAnimeListApiResponse response);
}