using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaUpdater.Infrastructure.Entities;

public partial class Source
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string BaseUrl { get; set; } = null!;

    [InverseProperty("Source")]
    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    [InverseProperty("Source")]
    public virtual ICollection<MangaSource> MangaSources { get; set; } = new List<MangaSource>();

    [InverseProperty("Source")]
    public virtual ICollection<UserChapter> UserChapters { get; set; } = new List<UserChapter>();
}
