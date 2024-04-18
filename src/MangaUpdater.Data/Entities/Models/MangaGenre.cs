using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MangaUpdater.Data.Entities.Models;

public sealed class MangaGenre
{    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Key]
    public required int MangaId { get; set; }
    
    [Key]
    public required int GenreId { get; set; }
    
    [ForeignKey("MangaId")]
    [InverseProperty("MangaGenres")]
    [JsonIgnore] 
    public Manga? Manga { get; set; }
    
    [ForeignKey("GenreId")]
    [InverseProperty("MangaGenres")]
    [JsonIgnore] 
    public Genre? Genre { get; set; }
}