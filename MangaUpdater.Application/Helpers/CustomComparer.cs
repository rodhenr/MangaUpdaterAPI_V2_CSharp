using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Helpers;

public class ChapterEqualityComparer : IEqualityComparer<Chapter>
{
    public bool Equals(Chapter? x, Chapter? y)
    {
        // Check for null and compare the properties
        return x != null && y != null &&
               x.MangaId == y.MangaId &&
               x.SourceId == y.SourceId &&
               x.Number == y.Number;
    }

    public int GetHashCode(Chapter obj)
    {
        // Generate a hash code based on the properties
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + obj.MangaId.GetHashCode();
            hash = hash * 23 + obj.SourceId.GetHashCode();
            hash = hash * 23 + obj.Number.GetHashCode();
            return hash;
        }
    }
}