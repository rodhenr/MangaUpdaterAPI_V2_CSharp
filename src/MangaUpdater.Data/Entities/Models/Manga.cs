using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

public sealed class Manga
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(200)]
    [Unicode(false)]
    public required string CoverUrl { get; set; }
    
    [StringLength(2000)]
    [Unicode(false)]
    public required string Synopsis { get; set; }
    
    [StringLength(50)]
    [Unicode(false)]
    public required string Type { get; set; }
    
    [DefaultValue(0)]
    public required int MyAnimeListId { get; set; }

    [InverseProperty("MangaId")]
    [JsonIgnore] 
    public IEnumerable<Chapter>? Chapters { get; set; }
    
    [InverseProperty("MangaId")]
    [JsonIgnore] 
    public IEnumerable<UserManga>? UserMangas { get; set; }
    
    [InverseProperty("MangaId")]
    [JsonIgnore] 
    public IEnumerable<MangaAuthor>? MangaAuthors { get; set; }
    
    [InverseProperty("MangaId")]
    [JsonIgnore] 
    public IEnumerable<MangaGenre>? MangaGenres { get; set; }
    
    [InverseProperty("MangaId")]
    [JsonIgnore] 
    public IEnumerable<MangaSource>? MangaSources { get; set; }
    
    [InverseProperty("MangaId")]
    [JsonIgnore] 
    public IEnumerable<MangaTitle>? MangaTitles { get; set; }
}