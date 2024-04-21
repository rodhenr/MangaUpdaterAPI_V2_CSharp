using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

public sealed class Source
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(50)]
    [Unicode(false)]
    public required string Name { get; set; }
    
    [StringLength(100)]
    [Unicode(false)]
    public required string BaseUrl { get; set; }

    [InverseProperty("Source")]
    [JsonIgnore]
    public List<UserChapter> UserChapter { get; set; } = [];
    
    [InverseProperty("Source")]
    [JsonIgnore] 
    public List<MangaSource> MangaSources { get; set; } = [];
    
    [InverseProperty("Source")]
    [JsonIgnore] 
    public List<Chapter> Chapters { get; set; } = [];
}