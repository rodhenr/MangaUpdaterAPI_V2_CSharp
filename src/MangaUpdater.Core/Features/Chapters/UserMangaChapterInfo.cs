using System.Globalization;
using MangaUpdater.Core.Dto;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public static class UserMangaChapterInfo
{
    public static IEnumerable<ChapterDto> GetUserChaptersState(IEnumerable<Chapter> chapters, List<UserMangaDto> userMangaInfo)
    {
        return chapters.Select(ch =>
            {
                var chapterInfo = userMangaInfo.FirstOrDefault(um => um.SourceId == ch.SourceId && um.MangaId == ch.MangaId);
                var isRead = float.Parse(ch.Number, CultureInfo.InvariantCulture) <= float.Parse(chapterInfo?.Number ?? "0", CultureInfo.InvariantCulture);

                return new ChapterDto(ch.Id, ch.Source.Id, ch.Source.Name, ch.Date, ch.Number, chapterInfo is not null, chapterInfo is not null && isRead);
            });
    }
}