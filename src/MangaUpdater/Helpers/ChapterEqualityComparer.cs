using MangaUpdater.Entities;

namespace MangaUpdater.Helpers;

public class ChapterEqualityComparer : IEqualityComparer<Chapter>
{
    public bool Equals(Chapter? x, Chapter? y)
    {
        return x != null && y != null &&
               x.MangaId == y.MangaId &&
               x.SourceId == y.SourceId &&
               x.Number == y.Number;
    }

    public int GetHashCode(Chapter obj)
    {
        unchecked
        {
            var hash = 17;
            
            hash = hash * 23 + obj.MangaId.GetHashCode();
            hash = hash * 23 + obj.SourceId.GetHashCode();
            hash = hash * 23 + obj.Number.GetHashCode();
            
            return hash;
        }
    }
}